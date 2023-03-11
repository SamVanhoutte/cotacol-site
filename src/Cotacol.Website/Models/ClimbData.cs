using Cotacol.Website.Models.CotacolApi;
using Newtonsoft.Json;

namespace Cotacol.Website.Models
{
    public class ClimbData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("province")]
        public string Province { get; set; }

        [JsonProperty("cotacol_points")]
        public int CotacolPoints { get; set; }

        [JsonProperty("distance")]
        public int Distance { get; set; }

        [JsonProperty("elevation_diff")]
        public int ElevationDiff { get; set; }

        [JsonProperty("bom_score")]
        public int BomScore { get; set; }
        
        [JsonIgnore]
        public string BombPrinted => BomScore > 0 ? BomScore.ToString() : "-";

        [JsonProperty("avg_grade")]
        public double AvgGrade { get; set; }

        [JsonProperty("surface")]
        public string Surface { get; set; }

        [JsonProperty("aliases")]
        public List<dynamic> Aliases { get; set; }

        [JsonProperty("strava_segment")]
        public string StravaSegment { get; set; }
        
        

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("polyline")]
        public string Polyline { get; set; }
        
        [JsonProperty("unique_users")]
        public int UniqueUsers { get; set; }
        [JsonProperty("total_attempts")]
        public int TotalAttempts { get; set; }
        [JsonProperty("local_legends")]
        public List<LocalLegend> LocalLegends { get; set; }
        public int CurrentLegendLimit => LocalLegends?.FirstOrDefault()?.Attempts ?? 5;
    }
}