using System.Text.Json.Serialization;

namespace Cotacol.Website.Models.CotacolApi
{
    public  class UpdateSegmentRequest
    {
        [JsonPropertyName("stravaSegmentId")]
        public string StravaSegmentId { get; set; }

        [JsonPropertyName("remark")]
        public string Remark { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("segmentConfidence")]
        public string SegmentConfidence { get; set; }

        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("polyline")] public string Polyline { get; set; }
        [JsonPropertyName("distance")]public long Distance { get; set; }

    }
}