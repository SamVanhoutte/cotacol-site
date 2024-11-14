using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public record StravaUserProfile
    {
        [JsonPropertyName("authorizationTokenValid")]
        public bool AuthorizationTokenValid { get; set; }

        [JsonPropertyName("activitiesCanBeRead")]
        public bool ActivitiesCanBeRead { get; set; }

        [JsonPropertyName("activityDescriptionCanBeUpdated")]
        public bool ActivityDescriptionCanBeUpdated { get; set; }

        [JsonPropertyName("activityReadStatus")]
        public string ActivityReadStatus { get; set; }

        [JsonPropertyName("activityDescriptionStatus")]
        public string ActivityDescriptionStatus { get; set; }

        [JsonPropertyName("exception")]
        public object Exception { get; set; }

        [JsonPropertyName("updateDescriptionFlag")]
        public bool UpdateDescriptionFlag { get; set; }

        [JsonPropertyName("stravaPermissions")]
        public object StravaPermissions { get; set; }

        [JsonPropertyName("tokenLastRefreshed")]
        public DateTimeOffset TokenLastRefreshed { get; set; }

        [JsonPropertyName("tokenIsExpired")]
        public bool TokenIsExpired { get; set; }

        [JsonPropertyName("tokenExpiration")]
        public DateTimeOffset TokenExpiration { get; set; }

        [JsonPropertyName("tokenScope")]
        public object TokenScope { get; set; }
    }
}