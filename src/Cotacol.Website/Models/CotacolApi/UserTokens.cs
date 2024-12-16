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
}