namespace CotacolApp.Models.CotacolApi
{
    public record UserSettings    {
        public int Status { get; set; } 
        public bool UpdateActivityDescription { get; set; } 
        public bool PremiumUser { get; set; } 
        public bool CotacolHunter { get; set; } 
        public bool EnableRouteMatching { get; set; } 
    }
}