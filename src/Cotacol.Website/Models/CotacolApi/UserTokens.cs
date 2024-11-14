using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public  class UserTokens
    {
        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("authorizationToken")]
        public string AuthorizationToken { get; set; }

        [JsonPropertyName("lastRefreshTime")]
        public string LastRefreshTime { get; set; }

        [JsonPropertyName("scope")]
        public dynamic Scope { get; set; }

        [JsonPropertyName("expiration")]
        public string Expiration { get; set; }

        [JsonPropertyName("isExpired")]
        public bool IsExpired { get; set; }
    }

    public class ActionHistory
    {
        public DateTime? IOSLogin { get; set; }
        public DateTime? AndroidLogin { get; set; }
        public DateTime? WebLogin { get; set; }
        public DateTime? FullSync { get; set; }
        public DateTime? ActivityWebhookSync { get; set; }
    }
    public class FullSyncStatus
    {
        public bool Succeeded { get; set; }
        public DateTime SyncTime { get; set; }
        public string ErrorDescription { get; set; }
        public string Trigger { get; set; }
        public string ErrorCode { get; set; }
    }
}