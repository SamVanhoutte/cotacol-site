

using System.Text.Json.Serialization;


namespace Cotacol.Website.Models.CotacolApi
{
    public class SystemStatus
    {
        [JsonPropertyName("stravaCurrentDayCalls")]
        public long StravaCurrentDayCalls { get; set; }

        [JsonPropertyName("stravaCurrentIntervalCalls")]
        public long StravaCurrentIntervalCalls { get; set; }

        [JsonPropertyName("stravaThrottlingValue")]
        public string StravaThrottlingValue { get; set; }

        [JsonPropertyName("activityQueueDepth")]
        public long ActivityQueueDepth { get; set; }

        [JsonPropertyName("serverTime")]
        public DateTimeOffset ServerTime { get; set; }

        [JsonPropertyName("throttlingInterval")]
        public TimeSpan ThrottlingInterval { get; set; }

        [JsonPropertyName("purgeOffset")]
        public DateTimeOffset PurgeOffset { get; set; }

        [JsonPropertyName("lastStatsUpdate")]
        public DateTimeOffset? LastStatsUpdate { get; set; }
        [JsonPropertyName("listedUserCount")]
        public long ListedUserCount { get; set; }

    }
}