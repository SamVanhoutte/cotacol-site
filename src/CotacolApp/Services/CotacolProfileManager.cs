using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CotacolApp.Interfaces;
using CotacolApp.Models.Identity;
using CotacolApp.Services.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CotacolApp.Services
{
    public class CotacolProfileManager : IUserProfileManager
    {
        private UserManager<CotacolUser> _userManager;
        private IHttpContextAccessor _contextAccessor;
        private SignInManager<CotacolUser> _signInManager;

        public CotacolProfileManager(IHttpContextAccessor contextAccessor,
            UserManager<CotacolUser> userManager, SignInManager<CotacolUser> signInManager)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
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
    }
}