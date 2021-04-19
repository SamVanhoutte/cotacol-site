using Newtonsoft.Json;

namespace CotacolApp.Models.CotacolApi
{
    public class UpdateCotacolRequest
    {
        [JsonProperty("polyline")]
        public string Polyline { get; set; }
    }
}