namespace CotacolApp.Interfaces
{
    public interface IUserProfileManager
    {
        string ProfilePicture { get; }
        string Email { get; }
        string UserId { get; }
        string UserName { get; }
        string FullName { get; }
        bool IsAuthenticated { get; }
    }
}