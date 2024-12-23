using System.Text.Json.Serialization;

namespace Cotacol.Website.Models.CotacolApi
{
    
    public class UserListRecord
    {
        [JsonPropertyName("profile")] public UserProfile Profile { get; set; }
        [JsonPropertyName("health")] public UserHealth Health { get; set; }
        [JsonPropertyName("stats")] public UserStats Stats { get; set; }

    }

    public class UserStats
    {
        public int AchievedBadges { get; set; }
        public int ColsDone { get; set; }
        public int TotalPoints { get; set; }
        public string Remark { get; set; }
    }
    public class UserHealth
    {
        [JsonPropertyName("userId")]
        public long UserId { get; set; }

        [JsonPropertyName("fullName")]
        public string FullName { get; set; }

        [JsonPropertyName("lastSynced")]
        public DateTime LastSynced { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("updateActivityDescription")]
        public bool UpdateActivityDescription { get; set; }

        [JsonPropertyName("privateProfile")]
        public bool PrivateProfile { get; set; }

        [JsonPropertyName("betaFeatures")]
        public bool BetaFeatures { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("totalActivities")]
        public long TotalActivities { get; set; }

        [JsonPropertyName("processedActivities")]
        public long ProcessedActivities { get; set; }

        [JsonPropertyName("syncActive")]
        public bool SyncActive { get; set; }

        [JsonPropertyName("syncStatus")]
        public string SyncStatus { get; set; }

        [JsonPropertyName("tokenExpired")]
        public bool? TokenExpired { get; set; }

        [JsonPropertyName("tokenExpiration")]
        public string TokenExpiration { get; set; }

        [JsonPropertyName("tokenScope")]
        public string TokenScope { get; set; }

        [JsonPropertyName("tokenRefresh")]
        public string TokenRefresh { get; set; }

        [JsonPropertyName("achievedBadges")]
        public long AchievedBadges { get; set; }

        [JsonPropertyName("colsDone")]
        public long ColsDone { get; set; }

        [JsonPropertyName("totalPoints")]
        public long TotalPoints { get; set; }

        [JsonPropertyName("persistenceService")]
        public string PersistenceService { get; set; }
        
        [JsonPropertyName("testUser")]
        public bool TestUser { get; set; }
    }
}
