using System.Text.Json.Serialization;

namespace Cotacol.Website.Models.CotacolApi.Strava;

public class StravaActivityDetailResponse
{
    [JsonPropertyName("activity")] public StravaActivity Activity { get; set; }

}