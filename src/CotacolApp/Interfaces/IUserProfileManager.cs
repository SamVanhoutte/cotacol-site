using System.Security.Claims;
using System.Threading.Tasks;

namespace CotacolApp.Interfaces
{
    public interface IUserProfileManager
    {
        Task AddStravaClaims(bool canUpdateActivity, bool canUpdateProfile, string userId);

        string ProfilePicture { get; }
        string Email { get; }
        string UserId { get; }
        string UserName { get; }
        string FullName { get; }
        bool IsAuthenticated { get; }
        bool CanUpdateDescription { get; }
        bool CanUpdateProfile{ get; }

    }
}