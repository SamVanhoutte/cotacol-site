using System;
using Newtonsoft.Json;

namespace CotacolApp.Models.CotacolApi
{
    public class SystemStatus
    {
        [JsonProperty("stravaCurrentDayCalls")]
        public long StravaCurrentDayCalls { get; set; }

        [JsonProperty("stravaCurrentIntervalCalls")]
        public long StravaCurrentIntervalCalls { get; set; }

        [JsonProperty("stravaThrottlingValue")]
        public string StravaThrottlingValue { get; set; }

        [JsonProperty("activityQueueDepth")]
        public long ActivityQueueDepth { get; set; }

        [JsonProperty("serverTime")]
        public DateTimeOffset ServerTime { get; set; }

        [JsonProperty("throttlingInterval")]
        public TimeSpan ThrottlingInterval { get; set; }

        [JsonProperty("purgeOffset")]
        public DateTimeOffset PurgeOffset { get; set; }

        [JsonProperty("lastStatsUpdate")]
        public DateTimeOffset LastStatsUpdate { get; set; }
        [JsonProperty("listedUserCount")]
        public long ListedUserCount { get; set; }

    }
}