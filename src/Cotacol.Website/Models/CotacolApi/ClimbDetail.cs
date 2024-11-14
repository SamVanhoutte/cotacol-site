using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public class ClimbDetail
    {
        [JsonPropertyName("cotacolpoints")]
        public long Cotacolpoints { get; set; }

        [JsonPropertyName("challenge")]
        public string Challenge { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("points")]
        public long Points { get; set; }

        [JsonPropertyName("province")]
        public string Province { get; set; }

        [JsonPropertyName("surface")]
        public string Surface { get; set; }
        [JsonPropertyName("bom_score")]
        public int BomScore { get; set; }

        [JsonPropertyName("polyline")]
        public string Polyline { get; set; }

        [JsonPropertyName("segment_confidence")]
        public string SegmentConfidence { get; set; }

        [JsonPropertyName("segments")]
        public List<SegmentDetail> Segments { get; set; }
        [JsonPropertyName("distance")]
        public long Distance { get; set; }
    }
}

