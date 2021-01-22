using System.Linq;
using System.Security.Claims;

namespace CotacolApp.Services
{
    public static class IdentityExtensionMethods
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            var userNameClaim = user.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Name));
            return userNameClaim?.Value;
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            var userNameClaim = user.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier));
            return userNameClaim?.Value;
        }
    }
}