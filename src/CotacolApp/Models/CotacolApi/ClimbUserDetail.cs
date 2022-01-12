using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace CotacolApp.Models.CotacolApi
{
    public class ClimbUserDetail
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

        [JsonProperty("avg_grade")]
        public double AvgGrade { get; set; }

        [JsonProperty("surface")]
        public string Surface { get; set; }
        [JsonProperty("bom_score")]
        public int BomScore { get; set; }

        [JsonProperty("strava_segment")]
        public string StravaSegment { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("polyline")]
        public string Polyline { get; set; }

        [JsonProperty("user_col_detail")]
        public UserColDetail UserColDetail { get; set; }

        [JsonProperty("stats")]
        public Stats Stats { get; set; }
    }
}