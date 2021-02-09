using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.SessionStorage;
using CotacolApp.Interfaces;
using CotacolApp.Models.Identity;
using CotacolApp.Services.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp;

namespace CotacolApp.Services
{
    public class CotacolProfileManager : IUserProfileManager
    {
        private UserManager<CotacolUser> _userManager;
        private IHttpContextAccessor _contextAccessor;

        public CotacolProfileManager(IHttpContextAccessor contextAccessor,
            UserManager<CotacolUser> userManager)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        private CotacolUser CurrentUser => _userManager.GetUserAsync(_contextAccessor.HttpContext.User).Result;
        private IList<Claim> CurrentClaims => _userManager.GetClaimsAsync(CurrentUser).Result;

        public async Task AddStravaClaims(bool canUpdateActivity, bool canUpdateProfile, string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var cotacolUser = _userManager.Users.FirstOrDefault(u => u.Id.Equals(userId));
                if (cotacolUser != null)
                {
                    var existingClaims = await _userManager.GetClaimsAsync(cotacolUser);
                    var claimsToRemove = existingClaims.Where(claim =>
                        claim.Type.Equals(IdentityExtensions.ActivityWriteClaim) ||
                        claim.Type.Equals(IdentityExtensions.ProfileUpdateClaim));
                    if(claimsToRemove.Any())
                    {
                        await _userManager.RemoveClaimsAsync(cotacolUser, claimsToRemove);
                    }
                    await _userManager.AddClaimAsync(cotacolUser,
                        new Claim(IdentityExtensions.ActivityWriteClaim, canUpdateActivity.ToString()));
                    await _userManager.AddClaimAsync(cotacolUser,
                        new Claim(IdentityExtensions.ProfileUpdateClaim, canUpdateProfile.ToString()));
                }
            }
            else
            {
                Console.WriteLine("User null");
            }
        }

        public bool IsAuthenticated => _contextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        public string ProfilePicture => CurrentUser?.ProfilePicture;
        public string Email => CurrentUser?.Email;
        public string UserId => CurrentUser?.Id;
        public string UserName => CurrentUser?.UserName;
        public string FullName => $"{CurrentUser?.FirstName} {CurrentUser?.LastName}";

        public bool CanUpdateDescription => CurrentClaims.Any(c =>
            c.Type.Equals(IdentityExtensions.ActivityWriteClaim, StringComparison.InvariantCultureIgnoreCase) &&
            c.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase));

        public bool CanUpdateProfile => CurrentClaims.Any(c =>
            c.Type.Equals(IdentityExtensions.ProfileUpdateClaim, StringComparison.InvariantCultureIgnoreCase) &&
            c.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase));
    }
}