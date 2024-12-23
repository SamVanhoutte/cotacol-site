using System.Text.Json.Serialization;

namespace Cotacol.Website.Models.CotacolApi
{
    public partial class ColActivity
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("startTime")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("duration")]
        public double Duration { get; set; }
    }
}