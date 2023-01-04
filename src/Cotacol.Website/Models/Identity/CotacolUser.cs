using Microsoft.AspNetCore.Identity;

namespace Cotacol.Website.Models.Identity
{
    public class CotacolUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        // public bool ActivityWritePermission { get; set; }
        // public bool ProfileWritePermission { get; set; }
    }
}