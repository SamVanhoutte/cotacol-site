using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace Cotacol.Website.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    [Route("account/signin")]
    public IActionResult SignIn()
    {
        var returnUrlQuery = Request.Query["ReturnUrl"];
        string returnUrl = returnUrlQuery.Any() ? returnUrlQuery.First() : "/";
        if (!User.Identity?.IsAuthenticated ?? false)
        {
            return Challenge(new AuthenticationProperties() {RedirectUri = returnUrl},
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

        return RedirectPermanent(returnUrl);
    }
    
    [Route("signout")]
    public async Task SignOut()
    {
        var scheme = IdentityConstants.ExternalScheme; // CookieAuthenticationDefaults.AuthenticationScheme; //"Identity.External";
        if (HttpContext.Request.Cookies.Any()) 
        {
            Console.WriteLine();
            var siteCookies = HttpContext.Request.Cookies.Where(c => 
                c.Key.Contains(".AspNetCore.", StringComparison.InvariantCultureIgnoreCase) || 
                c.Key.Contains("strava", StringComparison.InvariantCultureIgnoreCase) || 
                c.Key.Contains(scheme, StringComparison.InvariantCultureIgnoreCase) || 
                c.Key.Contains("Microsoft.Authentication", StringComparison.InvariantCultureIgnoreCase));
            foreach (var cookie in siteCookies)
            {
                Console.WriteLine($"Deleting cookie {cookie.Key}");
                Response.Cookies.Delete(cookie.Key);
            }
        }

        var prop = new AuthenticationProperties()
        {
            RedirectUri = "/"
        };
        // after signout this will redirect to your provided target
        await HttpContext.SignOutAsync(scheme, prop);
    }
}