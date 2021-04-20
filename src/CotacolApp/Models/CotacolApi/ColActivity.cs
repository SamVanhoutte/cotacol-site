using System;
using Newtonsoft.Json;

namespace CotacolApp.Models.CotacolApi
{
    public partial class ColActivity
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("startTime")]
        public DateTimeOffset StartTime { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("duration")]
        public long Duration { get; set; }
    }
}