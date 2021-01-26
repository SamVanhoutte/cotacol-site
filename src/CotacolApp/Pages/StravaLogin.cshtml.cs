using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CotacolApp.Pages
{
    public class StravaLogin : PageModel
    {
        public void OnGet()
        {
            Console.WriteLine("ok");
        }
        
        public async Task<IActionResult> OnPost()
        {
            // if (SignInManager<>.IsSignedIn(User))
            // {
            //     await SignInManager.SignOutAsync();
            // }

            return Redirect("~/");
        }
    }
}