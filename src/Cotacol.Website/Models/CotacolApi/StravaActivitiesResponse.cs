using System.Text.Json.Serialization;

namespace Cotacol.Website.Models.CotacolApi;

public class StravaActivitiesResponse
{
    [JsonPropertyName("totalActivities")] public long TotalActivities { get; set; }
    [JsonPropertyName("offset")] public int Offset { get; set; }
    [JsonPropertyName("finished")] public bool Finished { get; set; }
    [JsonPropertyName("existing")] public ActivityDetail[] ExistingActivities { get; set; }
    [JsonPropertyName("new")] public ActivityDetail[] NewActivities { get; set; }
}

public class ActivityDetail
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("userId")] public string UserId { get; set; }

    [JsonPropertyName("startTime")] public string StartTime { get; set; }

    [JsonPropertyName("syncTime")] public string SyncTime { get; set; }

    [JsonPropertyName("elevation")] public double Elevation { get; set; }

    [JsonPropertyName("distance")] public double Distance { get; set; }
}