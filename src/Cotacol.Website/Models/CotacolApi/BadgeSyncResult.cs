using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public record BadgeSyncResult
    {
        [JsonPropertyName("badgeId")]
        public string BadgeId { get; set; }

        [JsonPropertyName("completed")]
        public bool Completed { get; set; }

        [JsonPropertyName("remark")]
        public string Remark { get; set; }

        [JsonPropertyName("achievementDate")]
        public DateTimeOffset? AchievementDate { get; set; }

        [JsonPropertyName("numberColsAchieved")]
        public long NumberColsAchieved { get; set; }

        [JsonPropertyName("numberColsNeeded")]
        public long NumberColsNeeded { get; set; }

        [JsonPropertyName("colsAchieved")]
        public string[] ColsAchieved { get; set; }

        [JsonPropertyName("colsMissing")]
        public string[] ColsMissing { get; set; }
    }
}