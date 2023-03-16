using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public record BadgeSyncResult
    {
        [JsonProperty("badgeId")]
        public string BadgeId { get; set; }

        [JsonProperty("completed")]
        public bool Completed { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("achievementDate")]
        public DateTimeOffset? AchievementDate { get; set; }

        [JsonProperty("numberColsAchieved")]
        public long NumberColsAchieved { get; set; }

        [JsonProperty("numberColsNeeded")]
        public long NumberColsNeeded { get; set; }

        [JsonProperty("colsAchieved")]
        public string[] ColsAchieved { get; set; }

        [JsonProperty("colsMissing")]
        public string[] ColsMissing { get; set; }
    }
}