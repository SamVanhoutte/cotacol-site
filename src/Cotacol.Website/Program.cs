using System.Net.Http.Headers;
using System.Security.Claims;
using AeroBlazor;
using AeroBlazor.Configuration;
using AeroBlazor.Theming;
using Cotacol.Website.Interfaces;
using Cotacol.Website.Models.Identity;
using Cotacol.Website.Models.Settings;
using Cotacol.Website.Services;
using Cotacol.Website.Services.Extensions;
using Cotacol.Website.Services.Imaging;
using Cotacol.Website.Services.Maps;
using Cotacol.Website.Theming;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using MudBlazor.Services;

namespace Cotacol.Website
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var cfgBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.dev.json", true, true)
                .AddJsonFile($"local.settings.json", true, true)
                .AddEnvironmentVariables();

            var configuration = cfgBuilder.Build();

            var stravaSettings = new StravaSettings();
            configuration.GetSection("strava").Bind(stravaSettings);

            var mapsConfig = configuration.GetSection("Maps");
            var mapsOptions = new MapOptions();
            mapsConfig.Bind(mapsOptions);


            var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor()
                .AddHubOptions(cfg => cfg.MaximumReceiveMessageSize = 1048576); // For Blazor Google Maps
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(IdentityConstants.ExternalScheme, options =>
                {
                    options.LoginPath = "/account/signin";
                    options.LogoutPath = "/signout";
                })
                .AddOAuth(CookieAuthenticationDefaults.AuthenticationScheme, "Strava",
                    options =>
                    {
                        options.ClientId = stravaSettings.ClientId;
                        options.ClientSecret = stravaSettings.ClientOauthSecret;
                        options.CallbackPath = "/signin-strava"; //"/stravalogin"; //

                        options.SaveTokens = false; // Save the auth/refresh token for later retrieval

                        options.SignInScheme = IdentityConstants.ExternalScheme;
                        options.AuthorizationEndpoint = "https://www.strava.com/oauth/authorize";
                        options.TokenEndpoint = stravaSettings.AccessTokenUrl;
                        options.UserInformationEndpoint = "https://www.strava.com/api/v3/athlete";

                        options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "athlete:id");

                        options.Scope.Clear();
                        options.Scope.Add(
                            "read,read_all,activity:read_all,activity:read,activity:write,profile:write");

                        options.Events = new OAuthEvents
                        {
                            OnCreatingTicket = async context =>
                            {
                                try
                                {
                                    var x = context.TokenResponse.Response.RootElement.GetProperty("athlete");
                                    var request = new HttpRequestMessage(HttpMethod.Get,
                                        context.Options.UserInformationEndpoint);
                                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                    request.Headers.Authorization =
                                        new AuthenticationHeaderValue("Bearer", context.AccessToken);

                                    var tokens = context.Properties.GetTokens().ToList();

                                    var userSettings = context.AddClaims(x.ToString());
                                    tokens.AddRange(
                                        userSettings.Select(userSetting => new AuthenticationToken()
                                            {Name = userSetting.Key, Value = userSetting.Value}));
                                    context.Properties.StoreTokens(tokens);
                                }
                                catch (Exception e)
                                {
                                }
                            },
                            OnRedirectToAuthorizationEndpoint = ctx =>
                            {
                                if (ctx.Properties.Items.ContainsKey(StravaAuthenticationProperties.ApprovalPrompt))
                                {
                                    ctx.HttpContext.Response.Redirect(ctx.RedirectUri + "&approval_prompt=force");
                                }
                                else
                                {
                                    ctx.HttpContext.Response.Redirect(ctx.RedirectUri);
                                }

                                return Task.FromResult(0);
                            },
                            OnTicketReceived = async context =>
                            {
                                if (context.Request.Query.TryGetValue("scope", out var scope))
                                {
                                    context.Properties.Items.Add("Scope", scope);
                                }
                            }
                        };
                        options.Validate();
                    });
            builder.Services.AddHttpContextAccessor();
            InjectServices(builder);
            builder.Services.AddMudServices();
            builder.Services
                .Configure<CotacolApiSettings>(options => configuration.GetSection("api").Bind(options))
                .Configure<StravaSettings>(options => configuration.GetSection("strava").Bind(options))
                .Configure<MapSettings>(options => configuration.GetSection("maps").Bind(options))
                .Configure<AdminSettings>(options => configuration.GetSection("admin").Bind(options))
                .Configure<KeyVaultSettings>(options => configuration.GetSection("keyvault").Bind(options));

            builder.Services.AddAeroBlazorWebServices(options =>
            {
                options.EnableLocationServices = true;
                mapsOptions.DefaultMarkerIcon = "images/sfinx-map-icon.png";
                // options.ConfigureMaps(mapsOptions);
            });
            
            var app = builder.Build();
            app.UseAuthentication();
            app.UseAuthorization();

// Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCookiePolicy(new CookiePolicyOptions
                {MinimumSameSitePolicy = SameSiteMode.None, Secure = CookieSecurePolicy.None});
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }

        private static void InjectServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserProfileManager, CotacolProfileManager>();
            builder.Services.AddSingleton<ICotacolUserClient, CotacolApiUserClient>();
            builder.Services.AddSingleton<ICotacolClient, CotacolApiClient>();
            builder.Services.AddScoped<IMapService, MapListService>();
            builder.Services.AddScoped<IYearImageGenerator, ImgSharpYearImageGenerator>();
            builder.Services.AddSingleton<IThemeManager, CotacolThemeManager>();
        }
    }
}