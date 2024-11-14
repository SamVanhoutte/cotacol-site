using System.Text.Json.Serialization;

namespace Cotacol.Website.Models.CotacolApi
{
    public record UserBadgeStatus
    {
        [JsonPropertyName("badgeId")] public string BadgeId { get; set; }

        [JsonPropertyName("name")] public string Name { get; set; }

        [JsonPropertyName("type")] public string BadgeTypeName { get; set; }
        public BadgeType BadgeType => Enum.Parse<BadgeType>(BadgeTypeName);

        [JsonPropertyName("description")] public string Description { get; set; }

        [JsonPropertyName("completed")] public bool Completed { get; set; }

        [JsonPropertyName("achievementDate")] public DateTime? AchievementDate { get; set; }

        [JsonPropertyName("numberColsAchieved")] public long NumberColsAchieved { get; set; }

        [JsonPropertyName("numberColsNeeded")] public long NumberColsNeeded { get; set; }

        [JsonPropertyName("colsAchieved")] public IEnumerable<string> ColsAchieved { get; set; }

        [JsonPropertyName("colsMissing")] public IEnumerable<string> ColsMissing { get; set; }

        [JsonPropertyName("remark")] public string Remark { get; set; }

        [JsonPropertyName("validFrom")] public DateTime? ValidFrom { get; set; }

        [JsonPropertyName("validTo")] public DateTime? ValidTo { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public double Progress
        {
            get
            {
                if (NumberColsNeeded > 0)
                {
                    return ((double) NumberColsAchieved / (double) NumberColsNeeded);
                }

                return 1;
            }
        }
    }

    public enum BadgeType
    {
        Amount,
        List,
        Location
    };
}