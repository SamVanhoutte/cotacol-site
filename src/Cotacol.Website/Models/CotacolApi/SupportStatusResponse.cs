using System.Text.Json.Serialization;

namespace Cotacol.Website.Models.CotacolApi;

public class SupportStatusResponse
{
    [JsonPropertyName("openUserCases")]
    public int OpenUserCases { get; set; }
    
    [JsonPropertyName("openCases")]
    public int OpenCases { get; set; }
}