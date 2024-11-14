
using System.Text.Json.Serialization;

namespace Cotacol.Website.Models.CotacolApi
{
    public class ClimbUserDetail
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

        [JsonPropertyName("avg_grade")]
        public double AvgGrade { get; set; }

        [JsonPropertyName("surface")]
        public string Surface { get; set; }
        [JsonPropertyName("bom_score")]
        public int BomScore { get; set; }

        [JsonPropertyName("strava_segment")]
        public string StravaSegment { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("polyline")]
        public string Polyline { get; set; }

        [JsonPropertyName("user_col_detail")]
        public UserColDetail UserColDetail { get; set; }

        [JsonPropertyName("stats")]
        public Stats Stats { get; set; }
    }
}