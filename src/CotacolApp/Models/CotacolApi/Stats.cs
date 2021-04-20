using System.Collections.Generic;
using Newtonsoft.Json;

namespace CotacolApp.Models.CotacolApi
{
    public partial class Stats
    {
        [JsonProperty("cotacolId")]
        public long CotacolId { get; set; }

        [JsonProperty("cotacolName")]
        public string CotacolName { get; set; }

        [JsonProperty("uniqueUsers")]
        public long UniqueUsers { get; set; }

        [JsonProperty("totalAttempts")]
        public int TotalAttempts { get; set; }

        [JsonProperty("maximumUserAttempts")]
        public long MaximumUserAttempts { get; set; }

        [JsonProperty("localLegends")]
        public List<LocalLegend> LocalLegends { get; set; }

        [JsonProperty("userList")]
        public List<LocalLegend> UserList { get; set; }
    }
}