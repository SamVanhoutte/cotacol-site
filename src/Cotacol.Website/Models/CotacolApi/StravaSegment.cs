using System.Text.Json.Serialization;

namespace Cotacol.Website.Models.CotacolApi
{
    public  class StravaSegment
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("distance")]
        public double Distance { get; set; }

        [JsonPropertyName("polyline")]
        public string Polyline { get; set; }
    }
}