using System;
using Newtonsoft.Json;

namespace CotacolApp.Models.CotacolApi
{
    public record UserBadgeStatus
    {
        [JsonProperty("badgeId")] public string BadgeId { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("type")] public BadgeType BadgeType { get; set; }

        [JsonProperty("description")] public string Description { get; set; }

        [JsonProperty("completed")] public bool Completed { get; set; }

        [JsonProperty("achievementDate")] public DateTimeOffset? AchievementDate { get; set; }

        [JsonProperty("numberColsAchieved")] public long NumberColsAchieved { get; set; }

        [JsonProperty("numberColsNeeded")] public long NumberColsNeeded { get; set; }

        [JsonProperty("colsAchieved")] public object ColsAchieved { get; set; }

        [JsonProperty("colsMissing")] public object ColsMissing { get; set; }

        [JsonProperty("remark")] public string Remark { get; set; }

        [JsonProperty("validFrom")] public object ValidFrom { get; set; }

        [JsonProperty("validTo")] public object ValidTo { get; set; }

        [JsonIgnore]
        public double Progress
        {
            get
            {
                if (NumberColsNeeded > 0)
                {
                    return ((double)NumberColsAchieved / (double)NumberColsNeeded);
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