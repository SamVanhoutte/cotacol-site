using System.Text.Json.Serialization;

namespace Cotacol.Website.Models.CotacolApi
{
    public class BadgeOfMonthData
    {
        [JsonPropertyName("badgeType")] public string BadgeType { get; set; }
        [JsonPropertyName("year")] public int Year { get; set; }
        [JsonPropertyName("month")] public int Month { get; set; }
        [JsonPropertyName("startTime")] public DateTime? StartTime { get; set; }
        [JsonPropertyName("endTime")] public DateTime? EndTime { get; set; }
        [JsonPropertyName("badgeName")] public string BadgeName { get; set; }
        [JsonPropertyName("badgeDescription")] public string BadgeDescription { get; set; }
        [JsonPropertyName("cotacolIds")] public List<int> CotacolIds { get; set; }

        public string MonthKey => $"{Year}-{Month:00}";
    }
}