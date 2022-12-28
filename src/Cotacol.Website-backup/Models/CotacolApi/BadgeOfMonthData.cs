using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public class BadgeOfMonthData
    {
        [JsonProperty("badgeType")] public string BadgeType { get; set; }

        [JsonProperty("year")] public int Year { get; set; }

        [JsonProperty("month")] public int Month { get; set; }
        [JsonProperty("startTime")] public DateTime? StartTime { get; set; }
        [JsonProperty("endTime")] public DateTime? EndTime { get; set; }


        [JsonProperty("badgeName")] public string BadgeName { get; set; }

        [JsonProperty("badgeDescription")] public string BadgeDescription { get; set; }

        [JsonProperty("cotacolIds")] public List<int> CotacolIds { get; set; }

        public string MonthKey => $"{Year}-{Month:00}";
    }
}