using Newtonsoft.Json;

namespace CotacolApp.Models.CotacolApi
{
    public  class UpdateSegmentRequest
    {
        [JsonProperty("stravaSegmentId")]
        public string StravaSegmentId { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("segmentConfidence")]
        public string SegmentConfidence { get; set; }

        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("polyline")] public string Polyline { get; set; }
        [JsonProperty("distance")]public double Distance { get; set; }

    }
}