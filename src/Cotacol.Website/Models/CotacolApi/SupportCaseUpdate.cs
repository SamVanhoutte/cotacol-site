using System.Text.Json.Serialization;

namespace Cotacol.Website.Models.CotacolApi;

public class SupportCaseUpdate
{
    [JsonPropertyName("EmailAddress")]
    public string EmailAddress { get; set; }

    [JsonPropertyName("Description")]
    public string Description { get; set; }

    [JsonPropertyName("NewStatus")]
    public CaseStatus NewStatus { get; set; }
}