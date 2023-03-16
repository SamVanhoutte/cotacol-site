using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public class HomeStats
    {
        [JsonProperty("totalPoints")]
        public long TotalPoints { get; set; }

        [JsonProperty("uniqueCols")]
        public long UniqueCols { get; set; }

        [JsonProperty("users")]
        public long Users { get; set; }

        [JsonProperty("recentActivities")]
        public List<RecentActivity> RecentActivities { get; set; }
    }
}