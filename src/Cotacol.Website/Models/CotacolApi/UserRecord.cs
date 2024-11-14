using System.Text.Json.Serialization;

namespace Cotacol.Website.Models.CotacolApi
{
    public class UserRecord
    {
        [JsonPropertyName("userId")] public string UserId { get; set; }
        [JsonPropertyName("fullName")] public string FullName { get; set; }
        [JsonPropertyName("lastSynced")] public DateTime? LastSynced { get; set; }
        [JsonPropertyName("status")] public string Status { get; set; }
        [JsonPropertyName("updateActivityDescription")] public bool? UpdateActivityDescription { get; set; }
        [JsonPropertyName("privateProfile")] public bool? PrivateProfile { get; set; }
        [JsonPropertyName("betaFeatures")] public bool? BetaFeatures { get; set; }
        [JsonPropertyName("email")] public string Email { get; set; }
        [JsonPropertyName("totalActivities")] public long TotalActivities { get; set; }
        [JsonPropertyName("processedActivities")] public long ProcessedActivities { get; set; }
        [JsonPropertyName("syncActive")] public bool SyncActive { get; set; }
        [JsonPropertyName("syncStatus")] public string SyncStatus { get; set; }
        [JsonPropertyName("tokenExpired")] public bool? TokenExpired { get; set; }
        [JsonPropertyName("tokenExpiration")] public DateTime? TokenExpiration { get; set; }
        [JsonPropertyName("tokenScope")] public string TokenScope { get; set; }
        [JsonPropertyName("tokenRefresh")] public string TokenRefresh { get; set; }
    }
}