using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

public class SignOutController : Controller
{
    [HttpGet("/signout")]
    public async Task<IActionResult> SignOut()
    {
        await HttpContext.SignOutAsync("Identity.External");
        // await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/");
    }
}