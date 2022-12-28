using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cotacol.Website.Pages
{
    public class LoginModel : PageModel
    {
        public async Task OnGet(string returnUrl)
        {
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                var authenticationProperties = new AuthenticationProperties() {RedirectUri = returnUrl};
                await HttpContext.ChallengeAsync("Strava", authenticationProperties);
            }

            HttpContext.Response.Redirect("/");
        }
    }
}