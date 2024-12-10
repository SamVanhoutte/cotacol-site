using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public class UpdateCotacolRequest
    {
        [JsonPropertyName("polyline")]
        public string Polyline { get; set; }
    }
}