using System;
using Newtonsoft.Json;

namespace CotacolApp.Models.CotacolApi
{
    public  class UserTokens
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonProperty("authorizationToken")]
        public string AuthorizationToken { get; set; }

        [JsonProperty("lastRefreshTime")]
        public string LastRefreshTime { get; set; }

        [JsonProperty("scope")]
        public dynamic Scope { get; set; }

        [JsonProperty("expiration")]
        public string Expiration { get; set; }

        [JsonProperty("isExpired")]
        public bool IsExpired { get; set; }
    }
    public class ActionHistory
    {
        public DateTime IOSLogin { get; set; }
        public DateTime AndroidLogin { get; set; }
        public DateTime WebLogin { get; set; }
        public DateTime FullSync { get; set; }
        public DateTime ActivityWebhookSync { get; set; }
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