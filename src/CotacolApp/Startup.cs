using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CotacolApp.Areas.Identity;
using CotacolApp.Data;
using CotacolApp.Interfaces;
using CotacolApp.Models.CotacolApi;
using CotacolApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.CodeAnalysis.CSharp;

namespace CotacolApp
{
    public class Startup
    {
        private ICotacolClient _cotacolClient; 
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options =>
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
                    options.DefaultChallengeScheme    = "Strava";
                })
                .AddCookie()
                .AddOAuth("Strava","Strava",
                options =>
                {
                    //http://www.strava.com/oauth/authorize?client_id=4987&response_type=code&redirect_uri=https://cotacol-hunting-workers.azurewebsites.net/api/StravaTokenHttpTrigger&approval_prompt=force&state=cotacolll&scope=read,read_all,activity:read_all,activity:read,activity:write,profile:write
                    //https://www.strava.com/oauth/authorize?client_id=54778&scope=read&response_type=code&redirect_uri=https%3A%2F%2Flocalhost%3A5001%2Fsignin-strava&state=CfDJ8JoKNYGkoJRGiyjnLkJiaUXjUWR4qUReuvnpsCJPXslZ-CQ8zd_K_29IzJFzt3FOh_ARdAIChh1omQs_e-sTuGf06AgozflcyVSbLRtr4kenuHkd0i-TDE0OglQgZWCw4OdZ3D6flAsl1SvtBpZCUPa9h6OXn50HlcpZmnsz_LnoyLxviJlLkCU2Iipq9iWsZ6ze0kQLXgGSZF7yPZU_Xw0nGpPtelrgKW-az60jJJao-T9CdrEBBTcX-8TwPF-6kQrySYHmyvWfzxsTVBr7dp-bq4QGcF2-pZTdRtkym2SRwVYWQi9H4WNZW3zy8VMq1Q
                    options.ClientId = "54778"; //Configuration["strava-app-id"];
                    options.ClientSecret = "29dec80b378c4cb04be78d1defba2a7458b72b26"; //Configuration["strava-app-secret"];
                    options.CallbackPath = "/stravalogin"; //"/signin-strava";

                    options.SaveTokens = true; // Save the auth/refresh token for later retrieval
                    
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                    
                    options.AuthorizationEndpoint = "https://www.strava.com/oauth/authorize";
                    options.TokenEndpoint = "https://www.strava.com/api/v3/oauth/token";
                    options.UserInformationEndpoint = "https://www.strava.com/api/v3/athlete";
                    
                    options.Scope.Clear();
                    options.Scope.Add(
                        "read,read_all,activity:read_all,activity:read,activity:write,profile:write");

                    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
                    options.ClaimActions.MapJsonKey("FirstName", "firstname");
                    options.ClaimActions.MapJsonKey("LastName", "lastname");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Gender, "sex");
                    options.ClaimActions.MapJsonKey("ProfilePicture", "profile_medium");
                    
                    options.Events = new OAuthEvents
                    {
                        OnCreatingTicket = async context =>
                        {
                            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                            
                            var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                            response.EnsureSuccessStatusCode();
                            var claimsJson = await response.Content.ReadAsStringAsync();
                            //context.RunClaimActions(user.RootElement);
                            var userSettings = context.AddClaims(claimsJson);
                            
                            var tokens = context.Properties.GetTokens().ToList();
                            foreach (var authenticationToken in tokens)
                            {
                                Console.WriteLine($"{authenticationToken.Name} : {authenticationToken.Value}");
                            }
                            // register user here
                            Console.WriteLine($"Cotacol client instance found? {_cotacolClient!=null}");
                            if (_cotacolClient != null)
                            {
                                await _cotacolClient.SetupUserAsync(new UserSetupRequest
                                {
                                    CotacolHunter = true,
                                    UserId = GetValue( userSettings, "userId"),
                                    UserName = GetValue( userSettings, "userName"),
                                    FullName = $"{GetValue( userSettings, "firstName")} {GetValue( userSettings, "lastName")}",
                                    EmailAddress = GetValue( userSettings, "email"),
                                    PremiumUser = false,
                                    StravaRefreshToken = tokens.FirstOrDefault(token=>token.Name.Equals("refresh_token"))?.Value
                                });
                            }
                        }
                    };
                    options.Validate();
                });

            services.AddHttpContextAccessor();

            // Dependency injectoin
            services
                .AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>
                >();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<ICotacolClient, CotacolApiClient>();
            services.AddSingleton<ICotacolUserClient, CotacolApiUserClient>();
        }

        private string GetValue(IDictionary<string, string> settings, string setting, string defaultValue = null)
        {
            if (settings?.ContainsKey(setting)??false)
            {
                return settings[setting];
            }

            return defaultValue;
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ICotacolClient cotacolClient)
        {
            _cotacolClient = cotacolClient;
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