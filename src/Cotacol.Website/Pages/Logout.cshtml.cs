using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cotacol.Website
{
    public class LogoutModel : PageModel
    {
        [Authorize]
        public async Task OnGet()
        {
            var authenticationProperties = new AuthenticationProperties()
            {
                RedirectUri = "/",
                IsPersistent = true
            };

            await HttpContext.SignOutAsync("Strava", authenticationProperties);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}