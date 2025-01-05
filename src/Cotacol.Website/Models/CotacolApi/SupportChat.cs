using System.Text.Json.Serialization;

namespace Cotacol.Website.Models.CotacolApi;

public class SupportChat
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("createdOn")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("userId")]
    public string UserId { get; set; }
    [JsonPropertyName("userName")]
    public string? UserName { get; set; }

    [JsonPropertyName("attachments")]
    public string[] Attachments { get; set; }

    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; }
}