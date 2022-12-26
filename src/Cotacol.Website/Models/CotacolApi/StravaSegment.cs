using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public  class StravaSegment
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("distance")]
        public long Distance { get; set; }

        [JsonProperty("polyline")]
        public string Polyline { get; set; }
    }
}