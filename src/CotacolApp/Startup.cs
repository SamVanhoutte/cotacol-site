using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CotacolApp.Areas.Identity;
using CotacolApp.Data;
using CotacolApp.Interfaces;
using CotacolApp.Models.Identity;
using CotacolApp.Services;
using CotacolApp.Settings;
using MatBlazor;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Rewrite;

namespace CotacolApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
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
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<CotacolUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddRazorPages();
            services.AddServerSideBlazor();

            // Strava authentication
            services.AddAuthentication(options =>
                {
                    //options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    //options.DefaultSignInScheme       = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = "Strava";
                })
                .AddCookie()
                .AddOAuth("Strava", "Strava",
                    options =>
                    {
                        options.ClientId = stravaSettings.ClientId;
                        options.ClientSecret = stravaSettings.ClientOauthSecret;
                        options.CallbackPath = "/stravalogin"; //"/signin-strava";

                        options.SaveTokens = true; // Save the auth/refresh token for later retrieval

                        options.SignInScheme = IdentityConstants.ExternalScheme;

                        options.AuthorizationEndpoint = "https://www.strava.com/oauth/authorize";
                        options.TokenEndpoint = stravaSettings.AccessTokenUrl;
                        options.UserInformationEndpoint = "https://www.strava.com/api/v3/athlete";

                        options.Scope.Clear();
                        options.Scope.Add(
                            "read,read_all,activity:read_all,activity:read,activity:write,profile:write");

                        // options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                        // options.ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
                        // options.ClaimActions.MapJsonKey("FirstName", "firstname");
                        // options.ClaimActions.MapJsonKey("LastName", "lastname");
                        // options.ClaimActions.MapJsonKey(ClaimTypes.Gender, "sex");
                        // options.ClaimActions.MapJsonKey("ProfilePicture", "profile_medium");

                        options.Events = new OAuthEvents
                        {
                            OnCreatingTicket = async context =>
                            {
                                var request = new HttpRequestMessage(HttpMethod.Get,
                                    context.Options.UserInformationEndpoint);
                                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                request.Headers.Authorization =
                                    new AuthenticationHeaderValue("Bearer", context.AccessToken);

                                var response = await context.Backchannel.SendAsync(request,
                                    HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                                response.EnsureSuccessStatusCode();
                                var claimsJson = await response.Content.ReadAsStringAsync();
                                //context.RunClaimActions(user.RootElement);
                                var userSettings = context.AddClaims(claimsJson);
                            }
                        };
                        options.Validate();
                    });

            services.AddHttpContextAccessor();

            // Dependency injection
            services
                .AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<CotacolUser>
                >();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddOptions();
            services.AddScoped<IUserProfileManager, CotacolProfileManager>();
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<ICotacolClient, CotacolApiClient>();
            services.AddSingleton<ICotacolUserClient, CotacolApiUserClient>();
            services.AddSingleton<ISecretReader, KeyVaultReader>();
            services.AddMatBlazor();
            services
                .Configure<CotacolApiSettings>(options => configuration.GetSection("api").Bind(options))
                .Configure<StravaSettings>(options => configuration.GetSection("strava").Bind(options))
                .Configure<KeyVaultSettings>(options => configuration.GetSection("keyvault").Bind(options));
            
            // Inject HttpClient, required by MatBlazor components
            if (services.All(x => x.ServiceType != typeof(HttpClient)))
            {
                services.AddScoped(
                    s =>
                    {
                        var navigationManager = s.GetRequiredService<NavigationManager>();
                        return new HttpClient
                        {
                            BaseAddress = new Uri(navigationManager.BaseUri)
                        };
                    });
            }
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}