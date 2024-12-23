using System.Text.Json.Serialization;

namespace Cotacol.Website.Models.CotacolApi
{
    public class UpdateCotacolRequest
    {
        [JsonPropertyName("polyline")]
        public string Polyline { get; set; }
    }
}