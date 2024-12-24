using System.Text.Json.Serialization;

namespace Cotacol.Website.Models.CotacolApi;

public class SupportCaseRequest
{
    [JsonPropertyName("UserId")]
    public string UserId { get; set; }

    [JsonPropertyName("EmailAddress")]
    public string EmailAddress { get; set; }

    [JsonPropertyName("CaseType")]
    public string CaseType { get; set; }

    [JsonPropertyName("Description")]
    public string Description { get; set; }

    [JsonPropertyName("ReportContent")]
    public string ReportContent { get; set; }
}