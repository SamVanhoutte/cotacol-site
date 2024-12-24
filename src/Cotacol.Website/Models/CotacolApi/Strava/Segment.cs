using System.Text.Json.Serialization;

namespace Cotacol.Website.Models.CotacolApi.Strava;

public partial class Segment
{
    [JsonPropertyName("id")] public string Id { get; set; }

    // [JsonPropertyName("resource_state")] public long ResourceState { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }
    //
    // [JsonPropertyName("activity_type")] public string ActivityType { get; set; }
    //
    // [JsonPropertyName("distance")] public string Distance { get; set; }
    //
    // [JsonPropertyName("average_grade")] public string AverageGrade { get; set; }
    //
    // [JsonPropertyName("maximum_grade")] public string MaximumGrade { get; set; }
    //
    // [JsonPropertyName("elevation_high")] public string ElevationHigh { get; set; }
    //
    // [JsonPropertyName("elevation_low")] public string ElevationLow { get; set; }

    // [JsonPropertyName("start_latlng")] public string[] StartLatlng { get; set; }
    //
    // [JsonPropertyName("end_latlng")] public string[] EndLatlng { get; set; }
    //
    // [JsonPropertyName("elevation_profile")]
    // public object ElevationProfile { get; set; }
    //
    // [JsonPropertyName("climb_category")] public string ClimbCategory { get; set; }
    //
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // [JsonPropertyName("city")]
    // public string City { get; set; }
    //
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // [JsonPropertyName("state")]
    // public string State { get; set; }
    //
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // [JsonPropertyName("country")]
    // public string Country { get; set; }
    //
    // [JsonPropertyName("private")] public bool Private { get; set; }
    //
    // [JsonPropertyName("hazardous")] public bool Hazardous { get; set; }
    //
    // [JsonPropertyName("starred")] public bool Starred { get; set; }
}