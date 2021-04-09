using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Azure.Identity;
using Blazored.SessionStorage;
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
using CotacolApp.Models.Settings;
using CotacolApp.Services;
using CotacolApp.Services.Extensions;
using CotacolApp.Services.Maps;
using CotacolApp.Settings;
using MatBlazor;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Configuration;
using Serilog.Core;

namespace CotacolApp
{
    public class Startup
    {
        private ILogger<Startup> _logger;

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

            var logSettings = new LogSettings();
            configuration.GetSection("logging").Bind(logSettings);

            var apiSettings = new CotacolApiSettings();
            configuration.GetSection("api").Bind(apiSettings);

            var kvSettings = new KeyVaultSettings();
            configuration.GetSection("keyvault").Bind(kvSettings);

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
            services.AddServerSideBlazor().AddHubOptions(config => config.MaximumReceiveMessageSize = 1048576);
            if (!string.IsNullOrEmpty(kvSettings?.KeySasBlobUri))
            {
                services.AddDataProtection()
                    // .PersistKeysToFileSystem(new DirectoryInfo("/Users/samvanhoutte/Temp/bbb/ee"));
                    .PersistKeysToAzureBlobStorage(new Uri($"{kvSettings.KeySasBlobUri}"))
                    .ProtectKeysWithAzureKeyVault(new Uri(kvSettings.KeyKeyvaultUri), new DefaultAzureCredential());
                //        .PersistKeysToFileSystem(new DirectoryInfo(Configuration["KeyPersistenceLocation"]));
            }

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

                        options.Events = new OAuthEvents
                        {
                            OnCreatingTicket = async context =>
                            {
                                try
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
                                
                                    var tokens = context.Properties.GetTokens().ToList();
                                    tokens.AddRange(
                                        userSettings.Select(userSetting => new AuthenticationToken() 
                                            {Name = userSetting.Key, Value = userSetting.Value}));

                                    context.Properties.StoreTokens(tokens);
                                }
                                catch (Exception e)
                                {
                                }
                            },
                            OnRedirectToAuthorizationEndpoint =  ctx =>
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
                                    _logger.LogInformation($"Scope for login received: {scope.First()}");
                                    context.Properties.Items.Add("Scope", scope);
                                }
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
            services.AddBlazoredSessionStorage();
            services.AddOptions();
            services.AddLogging(loggingbuilder =>
            {
                loggingbuilder.ClearProvidersExceptFunctionProviders();
                if (!string.IsNullOrEmpty(logSettings.ApplicationInsightsInstrumentationKey))
                {
                    loggingbuilder.AddSerilog(
                        CreateLogConfiguration(logSettings.ApplicationInsightsInstrumentationKey));
                }
            });
            services.AddScoped<IUserProfileManager, CotacolProfileManager>();
            services.AddScoped<IMapService, MapListService>();
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<ICotacolClient, CotacolApiClient>();
            services.AddSingleton<ICotacolUserClient, CotacolApiUserClient>();
            services.AddSingleton<ISecretReader, KeyVaultReader>();
            services.AddMatBlazor();
            services
                .Configure<CotacolApiSettings>(options => configuration.GetSection("api").Bind(options))
                .Configure<StravaSettings>(options => configuration.GetSection("strava").Bind(options))
                .Configure<MapSettings>(options => configuration.GetSection("maps").Bind(options))
                .Configure<AdminSettings>(options => configuration.GetSection("admin").Bind(options))
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            _logger = logger;
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

            app.UseCookiePolicy(new CookiePolicyOptions{ MinimumSameSitePolicy = SameSiteMode.None, Secure =  CookieSecurePolicy.None});
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

        private Logger CreateLogConfiguration(string instrumentationKey)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithComponentName("Cotacol web app")
                .Enrich.WithVersion()
                .WriteTo.Console(LogEventLevel.Information)
                .WriteTo.AzureApplicationInsights(instrumentationKey, LogEventLevel.Verbose)
                .CreateLogger();

            return logger;
        }
    }
}