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
}