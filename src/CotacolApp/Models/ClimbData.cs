using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CotacolApp.Models
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
        public long CotacolPoints { get; set; }

        [JsonProperty("distance")]
        public long Distance { get; set; }

        [JsonProperty("elevation_diff")]
        public long ElevationDiff { get; set; }

        [JsonProperty("bom_score")]
        public int BomScore { get; set; }

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
    }
}