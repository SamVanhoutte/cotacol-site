using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public class HomeStats
    {
        [JsonPropertyName("totalPoints")]
        public long TotalPoints { get; set; }

        [JsonPropertyName("uniqueCols")]
        public long UniqueCols { get; set; }

        [JsonPropertyName("users")]
        public long Users { get; set; }

        [JsonPropertyName("recentActivities")]
        public List<RecentActivity> RecentActivities { get; set; }
    }
}