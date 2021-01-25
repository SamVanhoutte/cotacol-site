namespace CotacolApp.Models.CotacolApi
{
    public class UserSetupRequest
    {
            public string UserId { get; set; } 
            public string StravaRefreshToken { get; set; } 
            public bool UpdateActivityDescription { get; set; } 
            public string FullName { get; set; } 
            public string UserName { get; set; } 
            public string EmailAddress { get; set; } 
            public bool PremiumUser { get; set; } 
            public bool CotacolHunter { get; set; } 
    }
}