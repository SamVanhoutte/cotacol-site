using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(CotacolApp.Areas.Identity.IdentityHostingStartup))]
namespace CotacolApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}