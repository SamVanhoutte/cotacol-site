using System.Security.Claims;

namespace Cotacol.Website.Models.Support;

public class UserResult
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string Error { get; set; }
}