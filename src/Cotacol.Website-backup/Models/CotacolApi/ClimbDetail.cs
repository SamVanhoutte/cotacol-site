using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public class ClimbDetail
    {
        [JsonProperty("cotacolpoints")]
        public long Cotacolpoints { get; set; }

        [JsonProperty("challenge")]
        public string Challenge { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("points")]
        public long Points { get; set; }

        [JsonProperty("province")]
        public string Province { get; set; }

        [JsonProperty("surface")]
        public string Surface { get; set; }
        [JsonProperty("bom_score")]
        public int BomScore { get; set; }

        [JsonProperty("polyline")]
        public string Polyline { get; set; }

        [JsonProperty("segment_confidence")]
        public string SegmentConfidence { get; set; }

        [JsonProperty("segments")]
        public List<SegmentDetail> Segments { get; set; }
        [JsonProperty("distance", NullValueHandling = NullValueHandling.Ignore)]
        public long Distance { get; set; }
    }
}

