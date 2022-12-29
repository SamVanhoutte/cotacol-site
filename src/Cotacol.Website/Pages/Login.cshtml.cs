using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cotacol.Website.Pages
{
    public class LoginModel : PageModel
    {
        public async Task OnGet(string? returnUrl)
        {
            if ((!User.Identity?.IsAuthenticated ?? false))
            {
                var authenticationProperties = new AuthenticationProperties() {RedirectUri = returnUrl};
                await HttpContext.ChallengeAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationProperties);
                
                var claimsIdentity = new ClaimsIdentity(
                    User.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity), authenticationProperties);
                
            }
            else
            {
                HttpContext.Response.Redirect("/");
            }
        }
    }
}