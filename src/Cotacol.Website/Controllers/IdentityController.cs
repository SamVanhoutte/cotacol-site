using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Cotacol.Website.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    [Route("Account/SignIn")]
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
}