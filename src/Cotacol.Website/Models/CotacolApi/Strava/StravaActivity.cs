using System.Text.Json.Serialization;

namespace Cotacol.Website.Models.CotacolApi.Strava;

public class StravaActivity
{
    [JsonPropertyName("resource_state")] public long ResourceState { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("start_date")] public string StartDate { get; set; }
    [JsonPropertyName("segment_efforts")] public SegmentEffort[] SegmentEfforts { get; set; }
    // [JsonPropertyName("start_latlng")] public string[] StartLatlng { get; set; }
    // [JsonPropertyName("end_latlng")] public string[] EndLatlng { get; set; }
    // [JsonPropertyName("location_city")] public object LocationCity { get; set; }
    // [JsonPropertyName("location_state")] public object LocationState { get; set; }
    // [JsonPropertyName("location_country")] public string LocationCountry { get; set; }
    // [JsonPropertyName("achievement_count")]
    // public long AchievementCount { get; set; }
    // [JsonPropertyName("kudos_count")] public long KudosCount { get; set; }
    // [JsonPropertyName("comment_count")] public long CommentCount { get; set; }
    // [JsonPropertyName("athlete_count")] public long AthleteCount { get; set; }
    // [JsonPropertyName("photo_count")] public long PhotoCount { get; set; }
    // [JsonPropertyName("trainer")] public bool Trainer { get; set; }
    // [JsonPropertyName("commute")] public bool Commute { get; set; }
    // [JsonPropertyName("manual")] public bool Manual { get; set; }
    // [JsonPropertyName("private")] public bool Private { get; set; }
    // [JsonPropertyName("visibility")] public string Visibility { get; set; }
    // [JsonPropertyName("flagged")] public bool Flagged { get; set; }
    // [JsonPropertyName("gear_id")] public string GearId { get; set; }
    // [JsonPropertyName("from_accepted_tag")]
    // public bool FromAcceptedTag { get; set; }
    // [JsonPropertyName("upload_id_str")] public string UploadIdStr { get; set; }
    // [JsonPropertyName("average_speed")] public string AverageSpeed { get; set; }
    // [JsonPropertyName("max_speed")] public string MaxSpeed { get; set; }
    // [JsonPropertyName("average_watts")] public string AverageWatts { get; set; }
    // [JsonPropertyName("kilojoules")] public string Kilojoules { get; set; }
    // [JsonPropertyName("device_watts")] public bool DeviceWatts { get; set; }
    // [JsonPropertyName("has_heartrate")] public bool HasHeartrate { get; set; }
    //
    // [JsonPropertyName("heartrate_opt_out")]
    // public bool HeartrateOptOut { get; set; }
    //
    // [JsonPropertyName("display_hide_heartrate_option")]
    // public bool DisplayHideHeartrateOption { get; set; }
    //
    // [JsonPropertyName("elev_high")] public string ElevHigh { get; set; }
    //
    // [JsonPropertyName("elev_low")] public string ElevLow { get; set; }
    //
    // [JsonPropertyName("pr_count")] public string PrCount { get; set; }
    //
    // [JsonPropertyName("total_photo_count")]
    // public string TotalPhotoCount { get; set; }
    //
    // [JsonPropertyName("has_kudoed")] public bool HasKudoed { get; set; }
    //
    // [JsonPropertyName("suffer_score")] public object SufferScore { get; set; }
    //
    // [JsonPropertyName("description")] public string Description { get; set; }
    //
    // [JsonPropertyName("calories")] public string Calories { get; set; }
    //
    // [JsonPropertyName("prefer_perceived_exertion")]
    // public bool PreferPerceivedExertion { get; set; }
    //
    //
    // [JsonPropertyName("device_name")] public string DeviceName { get; set; }
    //
    // [JsonPropertyName("embed_token")] public string EmbedToken { get; set; }
    //
    // [JsonPropertyName("available_zones")] public string[] AvailableZones { get; set; }
    //
    // [JsonPropertyName("statusCode")] public long StatusCode { get; set; }
    //
    // [JsonPropertyName("status")] public long Status { get; set; }
    //
    // [JsonPropertyName("shouldRetry")] public bool ShouldRetry { get; set; }
    //
    // [JsonPropertyName("succeeded")] public bool Succeeded { get; set; }
    //
    // [JsonPropertyName("rateUsageString")] public object RateUsageString { get; set; }
    //
    // [JsonPropertyName("errorMessage")] public object ErrorMessage { get; set; }
}