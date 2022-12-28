namespace Cotacol.Website.Models.CotacolApi
{
    public record UserSettings    {
        public int Status { get; set; } 
        public bool UpdateActivityDescription { get; set; } 
        public bool PremiumUser { get; set; } 
        public bool CotacolHunter { get; set; } 
        public bool EnableRouteMatching { get; set; }
        public bool PrivateProfile { get; set; }
        public bool EnableBetaFeatures { get; set; }
        public string PersistenceService { get; set; }
    }
}