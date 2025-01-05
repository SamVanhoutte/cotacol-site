using System.Collections;
using System.Text.Json.Serialization;

namespace Cotacol.Website.Models.CotacolApi;

public class SupportCase
{
    [JsonPropertyName("caseId")]
    public Guid CaseId { get; set; }

    [JsonPropertyName("userId")]
    public string UserId { get; set; }
    [JsonPropertyName("userName")]
    public string? UserName { get; set; }

    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; }

    [JsonPropertyName("creationTime")]
    public DateTime CreationTime { get; set; }

    [JsonPropertyName("updatedTime")]
    public DateTime UpdatedTime { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }
    
    [JsonIgnore]
    public CaseStatus ActiveStatus {
        get => (CaseStatus)Status;
        set => Status = (int)value;
    }

    [JsonPropertyName("caseType")]
    public string CaseType { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("reportContent")]
    public string ReportContent { get; set; }

    [JsonPropertyName("chats")]
    public SupportChat[] Chats { get; set; }
}

public enum CaseStatus
{
    New = 0,
    WaitingForUser = -1,
    Solved=1,
    Closed=-2
}