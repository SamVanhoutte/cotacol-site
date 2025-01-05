using System.Text.Json.Serialization;
using Cotacol.Website.Models.CotacolApi;

namespace Cotacol.Website.Models
{
    public class ClimbData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("province")]
        public string Province { get; set; }

        [JsonPropertyName("cotacol_points")]
        public int CotacolPoints { get; set; }

        [JsonPropertyName("distance")]
        public int Distance { get; set; }

        [JsonPropertyName("elevation_diff")]
        public int ElevationDiff { get; set; }

        [JsonPropertyName("bom_score")]
        public int BomScore { get; set; }
        
        public string BombPrinted => BomScore > 0 ? BomScore.ToString() : "-";

        [JsonPropertyName("avg_grade")]
        public double AvgGrade { get; set; }

        [JsonPropertyName("surface")]
        public string Surface { get; set; }

        [JsonPropertyName("aliases")]
        public List<dynamic> Aliases { get; set; }

        [JsonPropertyName("strava_segment")]
        public string StravaSegment { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("polyline")]
        public string Polyline { get; set; }
        
        [JsonPropertyName("unique_users")]
        public int UniqueUsers { get; set; }
        [JsonPropertyName("total_attempts")]
        public int TotalAttempts { get; set; }
        [JsonPropertyName("local_legends")]
        public List<LocalLegend>? LocalLegends { get; set; }
        public int CurrentLegendLimit => LocalLegends?.FirstOrDefault()?.Attempts ?? 5;
    }
}