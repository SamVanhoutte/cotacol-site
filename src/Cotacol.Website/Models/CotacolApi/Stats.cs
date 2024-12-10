using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public partial class Stats
    {
        [JsonPropertyName("cotacolId")]
        public string CotacolId { get; set; }

        [JsonPropertyName("cotacolName")]
        public string CotacolName { get; set; }

        [JsonPropertyName("uniqueUsers")]
        public long UniqueUsers { get; set; }

        [JsonPropertyName("totalAttempts")]
        public int TotalAttempts { get; set; }

        [JsonPropertyName("maximumUserAttempts")]
        public long MaximumUserAttempts { get; set; }

        [JsonPropertyName("localLegends")]
        public List<LocalLegend> LocalLegends { get; set; }

        [JsonPropertyName("userList")]
        public List<LocalLegend> UserList { get; set; }
    }
}