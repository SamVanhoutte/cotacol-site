using System;
using Newtonsoft.Json;

namespace CotacolApp.Models.CotacolApi
{
    public class UserListRecord
    {
        [JsonProperty("userId")]
        public long UserId { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("lastSynced")]
        public DateTime LastSynced { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("updateActivityDescription")]
        public bool UpdateActivityDescription { get; set; }

        [JsonProperty("privateProfile")]
        public bool PrivateProfile { get; set; }

        [JsonProperty("betaFeatures")]
        public bool BetaFeatures { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("totalActivities")]
        public long TotalActivities { get; set; }

        [JsonProperty("processedActivities")]
        public long ProcessedActivities { get; set; }

        [JsonProperty("syncActive")]
        public bool SyncActive { get; set; }

        [JsonProperty("syncStatus")]
        public string SyncStatus { get; set; }

        [JsonProperty("tokenExpired")]
        public bool? TokenExpired { get; set; }

        [JsonProperty("tokenExpiration")]
        public string TokenExpiration { get; set; }

        [JsonProperty("tokenScope")]
        public string TokenScope { get; set; }

        [JsonProperty("tokenRefresh")]
        public string TokenRefresh { get; set; }

        [JsonProperty("achievedBadges")]
        public long AchievedBadges { get; set; }

        [JsonProperty("colsDone")]
        public long ColsDone { get; set; }

        [JsonProperty("totalPoints")]
        public long TotalPoints { get; set; }

        [JsonProperty("persistenceService")]
        public string PersistenceService { get; set; }
        
        [JsonProperty("testUser")]
        public bool TestUser { get; set; }
    }
}
