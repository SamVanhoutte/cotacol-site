using System.Security.Claims;
using Cotacol.Website.Interfaces;
using Cotacol.Website.Models.Settings;
using Cotacol.Website.Services.Extensions;
using Microsoft.Extensions.Options;

namespace Cotacol.Website.Services
{
    public class CotacolProfileManager : IUserProfileManager
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AdminSettings _adminSettings;
        private readonly CotacolApiSettings _apiSettings;


        public CotacolProfileManager(IHttpContextAccessor contextAccessor, IOptions<AdminSettings> adminSettings,
            IOptions<CotacolApiSettings> apiSettings)
        {
            _contextAccessor = contextAccessor;
            _adminSettings = adminSettings.Value;
            _apiSettings = apiSettings.Value;
        }

        private ClaimsPrincipal? CurrentUser => _contextAccessor.HttpContext?.User;

        public IEnumerable<Claim> GetCurrentClaims()
        {
            return CurrentUser?.Claims.ToList() ?? new List<Claim> { };
        }

        // public async Task<StravaClaimSettings> AddStravaClaims(bool canUpdateActivity, bool canUpdateProfile,
        //     string refreshToken, string userId)
        // {
        //     if (!string.IsNullOrEmpty(userId))
        //     {
        //         var cotacolUser = _userManager.Users.FirstOrDefault(u => u.Id.Equals(userId));
        //         if (cotacolUser != null)
        //         {
        //             var existingClaims = await _userManager.GetClaimsAsync(cotacolUser);
        //             var claimsToRemove = existingClaims.Where(claim =>
        //                 claim.Type.Equals("refresh_token") ||
        //                 claim.Type.Equals(IdentityExtensions.ActivityWriteClaim) ||
        //                 claim.Type.Equals(IdentityExtensions.ProfileUpdateClaim));
        //             if (claimsToRemove.Any())
        //             {
        //                 await _userManager.RemoveClaimsAsync(cotacolUser, claimsToRemove);
        //             }
        //
        //             if (!string.IsNullOrEmpty(refreshToken))
        //             {
        //                 await _userManager.AddClaimAsync(cotacolUser, new Claim("refresh_token", refreshToken));
        //             }
        //
        //             await _userManager.AddClaimAsync(cotacolUser,
        //                 new Claim(IdentityExtensions.ActivityWriteClaim, canUpdateActivity.ToString()));
        //             await _userManager.AddClaimAsync(cotacolUser,
        //                 new Claim(IdentityExtensions.ProfileUpdateClaim, canUpdateProfile.ToString()));
        //             return new StravaClaimSettings
        //             {
        //                 UpdateActivity = canUpdateActivity,
        //                 UpdateProfile = canUpdateProfile,
        //                 RefreshToken = refreshToken
        //             };
        //         }
        //     }
        //     else
        //     {
        //         Console.WriteLine("User null");
        //     }
        //
        //     return null;
        // }


        public async Task<string> GetRefreshTokenAsync()
        {
            var claims =  GetCurrentClaims();
            var tokenClaim = claims.FirstOrDefault(c =>
                c.Type.Equals("refresh_token", StringComparison.InvariantCultureIgnoreCase));
            return  tokenClaim?.Value;
        }

        public bool IsAuthenticated => _contextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        public string ProfilePicture => GetClaim(IdentityExtensions.ProfilePictureClaim);
        public string Email => GetClaim(IdentityExtensions.EmailClaim);
        public string UserId => GetClaim(IdentityExtensions.UserIdClaim);
        public bool IsAdmin => _adminSettings.IsAdmin(UserId);
        public string UserName => GetClaim(IdentityExtensions.UserNameClaim);
        public string FullName => $"{GetClaim(IdentityExtensions.FirstNameClaim)} {GetClaim(IdentityExtensions.LastNameClaim)}";

        public async Task<bool> CanUpdateDescriptionAsync()
        {
            var claims =  GetCurrentClaims();
            return claims.Any(c =>
                c.Type.Equals(IdentityExtensions.ActivityWriteClaim, StringComparison.InvariantCultureIgnoreCase) &&
                c.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<bool> CanUpdateProfileAsync()
        {
            var claims =  GetCurrentClaims();
            return claims.Any(c =>
                c.Type.Equals(IdentityExtensions.ProfileUpdateClaim, StringComparison.InvariantCultureIgnoreCase) &&
                c.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase));
        }

        private string GetClaim(string claimType)
        {
            var user = CurrentUser;
            if (user != null)
            {
                var requestedClaim = user.Claims.FirstOrDefault(c => c.Type.Equals(claimType));
                if (requestedClaim != null) return requestedClaim.Value;
            }

            return null;
        }

        public string GetLoginLink()
        {
            var redirectUrl = "Identity/Account/Login";

            if (!string.IsNullOrEmpty(_apiSettings?.RedirectDomain))
            {
                if (!_contextAccessor.HttpContext.Request.Host.Host
                        .Equals(_apiSettings.RedirectDomain, StringComparison.InvariantCultureIgnoreCase))
                {
                    redirectUrl = _apiSettings.LoginUrl;
                }
            }

            return redirectUrl;
        }
    }
}