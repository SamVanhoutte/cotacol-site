using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace CotacolApp.Services
{
    public static class IdentityExtensionMethods
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            var userNameClaim = user?.Claims?.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Name));
            return userNameClaim?.Value;
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            var userNameClaim = user?.Claims?.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier));
            return userNameClaim?.Value;
        }

        public static string GetUserName(this ExternalLoginInfo loginInfo)
        {
            return loginInfo?.Principal?.GetUserName();
        }

        public static string GetUserId(this ExternalLoginInfo loginInfo)
        {
            return loginInfo?.Principal?.GetUserId();
        }


        public static async Task<ExternalLoginInfo> GetLoginInfo(this ClaimsPrincipal user,
            SignInManager<IdentityUser> signInManager = null)
        {
            ExternalLoginInfo info = null;
            if (signInManager != null)
            {
                info = await signInManager.GetExternalLoginInfoAsync();
                if (info != null && !info.Principal.Claims.Any())
                {
                    info.Principal = user;
                }
            }


            if (info == null)
            {
                info = new ExternalLoginInfo(user,
                    "Strava", user.GetUserId(),
                    "Strava") {Principal = user};
            }

            return info;
        }

        public static void AddClaims(this OAuthCreatingTicketContext context, string claimsJson)
        {
            var data = JObject.Parse(claimsJson);


            var userId = (string) data["id"];
            if (!string.IsNullOrEmpty(userId))
            {
                context.Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId, ClaimValueTypes.String,
                    context.Options.ClaimsIssuer));
            }

            var firstName = (string) data["firstname"];
            if (!string.IsNullOrEmpty(firstName))
            {
                context.Identity.AddClaim(new Claim("FirstName", firstName, ClaimValueTypes.String,
                    context.Options.ClaimsIssuer));
            }

            var lastName = (string) data["lastname"];
            if (!string.IsNullOrEmpty(lastName))
            {
                context.Identity.AddClaim(new Claim("LastName", lastName, ClaimValueTypes.String,
                    context.Options.ClaimsIssuer));
            }

            var userName = (string) data["username"];
            if (!string.IsNullOrEmpty(userName))
            {
                context.Identity.AddClaim(new Claim(ClaimTypes.Name, userName, ClaimValueTypes.String,
                    context.Options.ClaimsIssuer));
            }

            var email = (string) data["email"];
            if (!string.IsNullOrEmpty(email))
            {
                context.Identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.String,
                    context.Options.ClaimsIssuer));
            }

            var pictureUrl = (string) data["profile_medium"];
            if (!string.IsNullOrEmpty(pictureUrl))
            {
                context.Identity.AddClaim(new Claim("profile-picture", pictureUrl, ClaimValueTypes.String,
                    context.Options.ClaimsIssuer));
            }
        }
    }
}