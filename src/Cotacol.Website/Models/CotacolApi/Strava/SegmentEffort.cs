using System.Text.Json.Serialization;

namespace Cotacol.Website.Models.CotacolApi.Strava;

public partial class SegmentEffort
{
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("segment")] public Segment Segment { get; set; }

    // [JsonPropertyName("resource_state")] public string ResourceState { get; set; }

    // [JsonPropertyName("name")] public string Name { get; set; }
    //
    // [JsonPropertyName("elapsed_time")] public string ElapsedTime { get; set; }
    //
    // [JsonPropertyName("moving_time")] public string MovingTime { get; set; }
    //
    // [JsonPropertyName("start_date")] public string StartDate { get; set; }
    //
    // [JsonPropertyName("start_date_local")] public string StartDateLocal { get; set; }
    //
    // [JsonPropertyName("distance")] public string Distance { get; set; }
    //
    // [JsonPropertyName("start_index")] public string StartIndex { get; set; }
    //
    // [JsonPropertyName("end_index")] public string EndIndex { get; set; }
    //
    // [JsonPropertyName("device_watts")] public bool? DeviceWatts { get; set; }


    // [JsonPropertyName("pr_rank")] public string? PrRank { get; set; }
    //
    // [JsonPropertyName("kom_rank")] public object KomRank { get; set; }
    //
    // [JsonPropertyName("hidden")] public bool Hidden { get; set; }
}