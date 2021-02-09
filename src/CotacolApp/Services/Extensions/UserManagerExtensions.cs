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