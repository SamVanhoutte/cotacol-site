using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public record StravaUserProfile
    {
        [JsonProperty("authorizationTokenValid")]
        public bool AuthorizationTokenValid { get; set; }

        [JsonProperty("activitiesCanBeRead")]
        public bool ActivitiesCanBeRead { get; set; }

        [JsonProperty("activityDescriptionCanBeUpdated")]
        public bool ActivityDescriptionCanBeUpdated { get; set; }

        [JsonProperty("activityReadStatus")]
        public string ActivityReadStatus { get; set; }

        [JsonProperty("activityDescriptionStatus")]
        public string ActivityDescriptionStatus { get; set; }

        [JsonProperty("exception")]
        public object Exception { get; set; }

        [JsonProperty("updateDescriptionFlag")]
        public bool UpdateDescriptionFlag { get; set; }

        [JsonProperty("stravaPermissions")]
        public object StravaPermissions { get; set; }

        [JsonProperty("tokenLastRefreshed")]
        public DateTimeOffset TokenLastRefreshed { get; set; }

        [JsonProperty("tokenIsExpired")]
        public bool TokenIsExpired { get; set; }

        [JsonProperty("tokenExpiration")]
        public DateTimeOffset TokenExpiration { get; set; }

        [JsonProperty("tokenScope")]
        public object TokenScope { get; set; }
    }
}