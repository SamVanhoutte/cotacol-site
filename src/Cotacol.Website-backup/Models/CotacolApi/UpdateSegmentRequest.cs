using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public  class UpdateSegmentRequest
    {
        [JsonProperty("stravaSegmentId")]
        public string StravaSegmentId { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("segmentConfidence", NullValueHandling = NullValueHandling.Ignore)]
        public string SegmentConfidence { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)] public string Name { get; set; }
        [JsonProperty("polyline", NullValueHandling = NullValueHandling.Ignore)] public string Polyline { get; set; }
        [JsonProperty("distance", NullValueHandling = NullValueHandling.Ignore)]public long Distance { get; set; }

    }
}