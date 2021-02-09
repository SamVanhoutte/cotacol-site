using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CotacolApp.Models.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace CotacolApp.Services.Extensions
{
    public static class IdentityExtensions
    {
        public const string ProfilePictureClaim = "ProfilePicture";
        public const string UserNameClaim = ClaimTypes.Name;
        public const string UserIdClaim = ClaimTypes.NameIdentifier;
        public const string EmailClaim = ClaimTypes.Email;
        public const string FirstNameClaim = "FirstName";
        public const string LastNameClaim = "LastName";
        public const string ActivityWriteClaim = "activity:write";
        public const string ProfileUpdateClaim = "profile:write";

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

        public static string GetClaim(this ClaimsPrincipal user, string claimType)
        {
            var userNameClaim = user?.Claims?.FirstOrDefault(claim => claim.Type.Equals(claimType));
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

        public static string GetClaim(this ExternalLoginInfo loginInfo, string claimType)
        {
            return loginInfo?.Principal?.GetClaim(claimType);
        }
        

        public static IDictionary<string, string> AddClaims(this OAuthCreatingTicketContext context, string claimsJson)
        {
            var data = JObject.Parse(claimsJson);

            var userSettings = new Dictionary<string, string>();

            var userId = (string) data["id"];
            if (!string.IsNullOrEmpty(userId))
            {
                userSettings.Add(UserIdClaim, userId);
                context.Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId, ClaimValueTypes.String,
                    context.Options.ClaimsIssuer));
            }


            if (context.Request.Query.TryGetValue("scope", out var scopeValues))
            {
                var scope = scopeValues.FirstOrDefault();
                if (!string.IsNullOrEmpty(scope))
                {
                    bool activityWritePermission = scope.Contains(ActivityWriteClaim);
                    userSettings.Add(ActivityWriteClaim, activityWritePermission.ToString());
                    context.Identity.AddClaim(new Claim(ActivityWriteClaim, activityWritePermission.ToString(),
                        ClaimValueTypes.String, context.Options.ClaimsIssuer));

                    bool profileWritePermission = scope.Contains(ProfileUpdateClaim);
                    userSettings.Add(ProfileUpdateClaim, profileWritePermission.ToString());
                    context.Identity.AddClaim(new Claim(ProfileUpdateClaim, profileWritePermission.ToString(),
                        ClaimValueTypes.String, context.Options.ClaimsIssuer));
                }
            }

            var firstName = (string) data["firstname"];
            if (!string.IsNullOrEmpty(firstName))
            {
                userSettings.Add(FirstNameClaim, firstName);
                context.Identity.AddClaim(new Claim(FirstNameClaim, firstName, ClaimValueTypes.String,
                    context.Options.ClaimsIssuer));
            }

            var lastName = (string) data["lastname"];
            if (!string.IsNullOrEmpty(lastName))
            {
                userSettings.Add(LastNameClaim, lastName);
                context.Identity.AddClaim(new Claim(LastNameClaim, lastName, ClaimValueTypes.String,
                    context.Options.ClaimsIssuer));
            }

            var userName = (string) data["username"];
            if (!string.IsNullOrEmpty(userName))
            {
                userSettings.Add(UserNameClaim, userName);
                context.Identity.AddClaim(new Claim(ClaimTypes.Name, userName, ClaimValueTypes.String,
                    context.Options.ClaimsIssuer));
            }

            var email = (string) data["email"];
            if (!string.IsNullOrEmpty(email))
            {
                userSettings.Add(EmailClaim, email);
                context.Identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.String,
                    context.Options.ClaimsIssuer));
            }

            var pictureUrl = (string) data["profile_medium"];
            if (!string.IsNullOrEmpty(pictureUrl))
            {
                userSettings.Add(ProfilePictureClaim, pictureUrl);
                context.Identity.AddClaim(new Claim(ProfilePictureClaim, pictureUrl, ClaimValueTypes.String,
                    context.Options.ClaimsIssuer));
            }

            return userSettings;
        }
    }
}