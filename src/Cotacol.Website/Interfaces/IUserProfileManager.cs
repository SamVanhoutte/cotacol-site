using System.Security.Claims;

namespace Cotacol.Website.Interfaces
{
    public interface IUserProfileManager
    {
        // Task<StravaClaimSettings> AddStravaClaims(bool canUpdateActivity, bool canUpdateProfile, string refreshToken, string userId);
        IEnumerable<Claim> GetCurrentClaims();
        Task<string> GetRefreshTokenAsync();
        string ProfilePicture { get; }
        string Email { get; }
        string UserId { get; }
        bool IsAdmin { get; }
        string UserName { get; }
        string FullName { get; }
        bool IsAuthenticated { get; }
        Task<bool> CanUpdateDescriptionAsync();
        Task<bool> CanUpdateProfileAsync();
        string GetLoginLink();
    }
}