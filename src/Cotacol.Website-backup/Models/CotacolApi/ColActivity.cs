using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public partial class ColActivity
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("duration")]
        public long Duration { get; set; }
    }
}