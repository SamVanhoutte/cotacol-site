using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public partial class SegmentDetail
    {
        [JsonPropertyName("segmentid")]
        public string? Segmentid { get; set; }

        [JsonPropertyName("isofficial")]
        public bool Isofficial { get; set; }

        [JsonPropertyName("validfrom")]
        public object Validfrom { get; set; }

        [JsonPropertyName("validto")]
        public DateTime? Validto { get; set; }

        [JsonPropertyName("userid")]
        public string? Userid { get; set; }

        [JsonPropertyName("remark")]
        public string Remark { get; set; }

        [JsonPropertyName("confidence")]
        public string Confidence { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("polyline")]
        public string Polyline { get; set; }

        [JsonPropertyName("distance")]
        public long Distance { get; set; }
    }
}