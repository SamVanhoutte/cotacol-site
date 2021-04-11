using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CotacolApp.Interfaces;
using CotacolApp.Models.Identity;
using CotacolApp.Models.Settings;
using CotacolApp.Services.Extensions;
using CotacolApp.Settings;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CotacolApp.Services
{
    public class CotacolProfileManager : IUserProfileManager
    {
        private UserManager<CotacolUser> _userManager;
        private IHttpContextAccessor _contextAccessor;
        private SignInManager<CotacolUser> _signInManager;
        private AdminSettings _adminSettings;
        private CotacolApiSettings _apiSettings;
        private IHttpContextAccessor _httpContextAccessor;


        public CotacolProfileManager(IHttpContextAccessor contextAccessor, IOptions<AdminSettings> adminSettings,
            UserManager<CotacolUser> userManager, SignInManager<CotacolUser> signInManager, 
            IHttpContextAccessor httpContextAccessor, IOptions<CotacolApiSettings> apiSettings)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
            _adminSettings = adminSettings.Value;
            _apiSettings = apiSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        private CotacolUser CurrentUser => _userManager.GetUserAsync(_contextAccessor.HttpContext.User).Result;

        private async Task<IList<Claim>> GetCurrentClaimsAsync()
        {
            return await _userManager.GetClaimsAsync(CurrentUser);
        }
        public async Task<StravaClaimSettings> AddStravaClaims(bool canUpdateActivity, bool canUpdateProfile,
            string refreshToken, string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var cotacolUser = _userManager.Users.FirstOrDefault(u => u.Id.Equals(userId));
                if (cotacolUser != null)
                {
                    var existingClaims = await _userManager.GetClaimsAsync(cotacolUser);
                    var claimsToRemove = existingClaims.Where(claim =>
                        claim.Type.Equals("refresh_token") ||
                        claim.Type.Equals(IdentityExtensions.ActivityWriteClaim) ||
                        claim.Type.Equals(IdentityExtensions.ProfileUpdateClaim));
                    if (claimsToRemove.Any())
                    {
                        await _userManager.RemoveClaimsAsync(cotacolUser, claimsToRemove);
                    }

                    if (!string.IsNullOrEmpty(refreshToken))
                    {
                        await _userManager.AddClaimAsync(cotacolUser, new Claim("refresh_token", refreshToken));
                    }
                    await _userManager.AddClaimAsync(cotacolUser,
                        new Claim(IdentityExtensions.ActivityWriteClaim, canUpdateActivity.ToString()));
                    await _userManager.AddClaimAsync(cotacolUser,
                        new Claim(IdentityExtensions.ProfileUpdateClaim, canUpdateProfile.ToString()));
                    return new StravaClaimSettings
                    {
                        UpdateActivity = canUpdateActivity,
                        UpdateProfile = canUpdateProfile,
                        RefreshToken = refreshToken
                    };
                }
            }
            else
            {
                Console.WriteLine("User null");
            }

            return null;
        }

        public async Task<string> GetRefreshTokenAsync()
        {
            var claims = await GetCurrentClaimsAsync();
            var tokenClaim = claims.FirstOrDefault(c => c.Type.Equals("refresh_token", StringComparison.InvariantCultureIgnoreCase) );
            return tokenClaim?.Value;
        }

        public bool IsAuthenticated => _contextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        public string ProfilePicture => CurrentUser?.ProfilePicture;
        public string Email => CurrentUser?.Email;
        public string UserId => CurrentUser?.Id;
        public bool IsAdmin => _adminSettings.IsAdmin(UserId);
        public string UserName => CurrentUser?.UserName;
        public string FullName => $"{CurrentUser?.FirstName} {CurrentUser?.LastName}";

        public async Task<bool> CanUpdateDescriptionAsync()
        {
            var claims = await GetCurrentClaimsAsync();
            return claims.Any(c =>
                c.Type.Equals(IdentityExtensions.ActivityWriteClaim, StringComparison.InvariantCultureIgnoreCase) &&
                c.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<bool> CanUpdateProfileAsync()
        {
            var claims = await GetCurrentClaimsAsync();
            return claims.Any(c =>
                c.Type.Equals(IdentityExtensions.ProfileUpdateClaim, StringComparison.InvariantCultureIgnoreCase) &&
                c.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase));
        }

        public string GetLoginLink()
        {
            var redirectUrl = "Identity/Account/Login";
        
            if (!string.IsNullOrEmpty(_apiSettings?.RedirectDomain))
            {
                if (!_httpContextAccessor.HttpContext.Request.Host.Host
                    .Equals(_apiSettings.RedirectDomain, StringComparison.InvariantCultureIgnoreCase))
                {
                    redirectUrl = _apiSettings.LoginUrl;
                }
            }

            return redirectUrl;
        }
    }
}