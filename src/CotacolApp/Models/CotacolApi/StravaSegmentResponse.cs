using Newtonsoft.Json;

namespace CotacolApp.Models.CotacolApi
{
    public  class StravaSegmentResponse
    {
        [JsonProperty("segment")]
        public StravaSegment Segment { get; set; }

        [JsonProperty("statusCode")]
        public string StatusCode { get; set; }

        [JsonProperty("shouldRetry")]
        public bool ShouldRetry { get; set; }

        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }

        [JsonProperty("retryTimespan")]
        public string RetryTimespan { get; set; }

        [JsonProperty("rateUsageString")]
        public string RateUsageString { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}