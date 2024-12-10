using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public  class StravaSegment
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("distance")]
        public long Distance { get; set; }

        [JsonPropertyName("polyline")]
        public string Polyline { get; set; }
    }
}