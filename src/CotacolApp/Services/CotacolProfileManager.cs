using CotacolApp.Interfaces;
using CotacolApp.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CotacolApp.Services
{
    public class CotacolProfileManager : IUserProfileManager
    {
        private UserManager<CotacolUser> _userManager;
        private IHttpContextAccessor _contextAccessor;

        public CotacolProfileManager(IHttpContextAccessor contextAccessor, UserManager<CotacolUser> userManager)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        private CotacolUser CurrentUser => _userManager.GetUserAsync(_contextAccessor.HttpContext.User).Result;

        public bool IsAuthenticated => _contextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        public string ProfilePicture => CurrentUser?.ProfilePicture;
        public string Email  => CurrentUser?.Email;
        public string UserId  => CurrentUser?.Id;
        public string UserName  => CurrentUser?.UserName;
        public string FullName => $"{CurrentUser?.FirstName} {CurrentUser?.LastName}";
    }
}