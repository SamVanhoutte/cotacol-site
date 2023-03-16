using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public class UpdateCotacolRequest
    {
        [JsonProperty("polyline")]
        public string Polyline { get; set; }
    }
}