using System.Linq;
using System.Threading.Tasks;
using CotacolApp.Interfaces;
using CotacolApp.Models.CotacolApi;
using CotacolApp.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace CotacolApp.Services.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task CreateCotacolUser(this CotacolUser user, ExternalLoginInfo info,
            ICotacolClient _cotacolClient)
        {
            // register user here
            if (_cotacolClient != null)
            {
                var configuredScope = info.GetPermissions();
                var request = new UserSetupRequest
                {
                    CotacolHunter = true,
                    UserId = user.Id,
                    StravaRefreshToken = info.AuthenticationTokens
                        .FirstOrDefault(token => token.Name.Equals("refresh_token"))?.Value,
                    UserName = user.UserName,
                    FullName = $"{user.FirstName} {user.LastName}",
                    EmailAddress = user.Email,
                    PremiumUser = false
                };
                if (configuredScope.Contains("activity:write"))
                {
                    request.UpdateActivityDescription = true;
                }

                await _cotacolClient.SetupUserAsync(request);
            }
        }
        
        public static string GetPermissions(this ExternalLoginInfo info)
        {
            var scope = "";
            if (info.AuthenticationProperties.Parameters.TryGetValue("scope", out var scopeValue))
            {
                scope = scopeValue.ToString();
            }

            return scope;
        }

        public static Task<CotacolUser> GetUser(this ExternalLoginInfo info)
        {
            var user = new CotacolUser
            {
                Id = info.GetUserId(),
                FirstName = info.Principal.GetClaim(IdentityExtensions.FirstNameClaim),
                LastName = info.Principal.GetClaim(IdentityExtensions.LastNameClaim),
                ProfilePicture = info.Principal.GetClaim(IdentityExtensions.ProfilePictureClaim),
                // ActivityWritePermission = bool.Parse( info.Principal.GetClaim(IdentityExtensions.ActivityWriteClaim)),
                // ProfileWritePermission = bool.Parse( info.Principal.GetClaim(IdentityExtensions.ProfileUpdateClaim)),
            };
            return Task.FromResult(user);
        }
    }
}