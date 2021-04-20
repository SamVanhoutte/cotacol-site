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
        public long CotacolPoints { get; set; }

        [JsonProperty("distance")]
        public long Distance { get; set; }

        [JsonProperty("elevation_diff")]
        public long ElevationDiff { get; set; }

        [JsonProperty("avg_grade")]
        public long AvgGrade { get; set; }

        [JsonProperty("surface")]
        public string Surface { get; set; }

        [JsonProperty("strava_segment")]
        public long StravaSegment { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

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