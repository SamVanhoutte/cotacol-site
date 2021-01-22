using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.AspNetCore.Authentication.OAuth;

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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            
            // Strava authentication
             services.AddAuthentication().AddOAuth("Strava",
                oAuthOptions =>
                {
                    //http://www.strava.com/oauth/authorize?client_id=4987&response_type=code&redirect_uri=https://cotacol-hunting-workers.azurewebsites.net/api/StravaTokenHttpTrigger&approval_prompt=force&state=cotacolll&scope=read,read_all,activity:read_all,activity:read,activity:write,profile:write
                    //https://www.strava.com/oauth/authorize?client_id=54778&scope=read&response_type=code&redirect_uri=https%3A%2F%2Flocalhost%3A5001%2Fsignin-strava&state=CfDJ8JoKNYGkoJRGiyjnLkJiaUXjUWR4qUReuvnpsCJPXslZ-CQ8zd_K_29IzJFzt3FOh_ARdAIChh1omQs_e-sTuGf06AgozflcyVSbLRtr4kenuHkd0i-TDE0OglQgZWCw4OdZ3D6flAsl1SvtBpZCUPa9h6OXn50HlcpZmnsz_LnoyLxviJlLkCU2Iipq9iWsZ6ze0kQLXgGSZF7yPZU_Xw0nGpPtelrgKW-az60jJJao-T9CdrEBBTcX-8TwPF-6kQrySYHmyvWfzxsTVBr7dp-bq4QGcF2-pZTdRtkym2SRwVYWQi9H4WNZW3zy8VMq1Q
                    oAuthOptions.ClientId = "54778"; //Configuration["strava-app-id"];
                    oAuthOptions.ClientSecret =
                        "29dec80b378c4cb04be78d1defba2a7458b72b26"; //Configuration["strava-app-secret"];
                    oAuthOptions.Scope.Clear();
                    oAuthOptions.Scope.Add(
                        "read,read_all,activity:read_all,activity:read,activity:write,profile:write");
                    oAuthOptions.CallbackPath = "/cols"; //"/signin-strava";
                    oAuthOptions.AuthorizationEndpoint = "https://www.strava.com/oauth/authorize";
                    oAuthOptions.TokenEndpoint = "https://www.strava.com/api/v3/oauth/token";
                    //oAuthOptions.SignInScheme = IdentityConstants.ExternalScheme;
                    oAuthOptions.Events = new OAuthEvents()
                    {
                        OnRemoteFailure = loginFailureHandler =>
                        {
                            Console.WriteLine("Remote Error");
                            Console.WriteLine(loginFailureHandler.Failure.Message);
                            var authProperties =
                                oAuthOptions.StateDataFormat.Unprotect(loginFailureHandler.Request.Query["state"]);
                            loginFailureHandler.Response.Redirect("/Identity/Account/Login");
                            loginFailureHandler.HandleResponse();
                            return Task.FromResult(0);
                        }, 
                        OnAccessDenied = handler =>
                        {
                            Console.WriteLine(handler.Response.StatusCode);
                            return Task.FromResult(0);
                        }
                    };
                    oAuthOptions.Validate();
                });
            
            
            
            // Dependency injectoin
            services
                .AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>
                >();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddSingleton<WeatherForecastService>();
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