using System;
using Newtonsoft.Json;

namespace CotacolApp.Models.CotacolApi
{
    public partial class SegmentDetail
    {
        [JsonProperty("segmentid")]
        public string? Segmentid { get; set; }

        [JsonProperty("isofficial")]
        public bool Isofficial { get; set; }

        [JsonProperty("validfrom")]
        public object Validfrom { get; set; }

        [JsonProperty("validto")]
        public DateTimeOffset? Validto { get; set; }

        [JsonProperty("userid")]
        public string? Userid { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("confidence")]
        public string Confidence { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("polyline")]
        public string Polyline { get; set; }

        [JsonProperty("distance")]
        public long Distance { get; set; }
    }
}