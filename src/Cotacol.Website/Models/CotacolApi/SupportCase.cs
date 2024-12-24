using System.Text.Json.Serialization;

namespace Cotacol.Website.Models.CotacolApi;

public class SupportCase
{
    [JsonPropertyName("caseId")]
    public Guid CaseId { get; set; }

    [JsonPropertyName("userId")]
    public string UserId { get; set; }

    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; }

    [JsonPropertyName("creationTime")]
    public string CreationTime { get; set; }

    [JsonPropertyName("updatedTime")]
    public string UpdatedTime { get; set; }

    [JsonPropertyName("status")]
    public long Status { get; set; }

    [JsonPropertyName("caseType")]
    public string CaseType { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("reportContent")]
    public string ReportContent { get; set; }
}