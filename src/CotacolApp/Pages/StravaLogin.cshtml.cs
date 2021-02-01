using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CotacolApp.Pages
{
    public class StravaLogin : PageModel
    {
        private ILogger<StravaLogin> _logger;

        public StravaLogin(ILogger<StravaLogin> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
            _logger?.LogInformation($"StravaLogin endpoint called (GET)");
            Console.WriteLine("ok");
        }
        
        public async Task<IActionResult> OnPost()
        {
            // if (SignInManager<>.IsSignedIn(User))
            // {
            //     await SignInManager.SignOutAsync();
            // }
            _logger?.LogInformation($"StravaLogin endpoint called (POST)");

            return Redirect("~/");
        }
    }
}