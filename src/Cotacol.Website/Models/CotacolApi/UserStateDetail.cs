using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cotacol.Website.Models.CotacolApi
{
    public class UserStateDetail
    {
        [JsonPropertyName("tokens")] public UserTokens UserTokens { get; set; }

        [JsonPropertyName("state")] public JObject State { get; set; }

        [JsonPropertyName("syncStatus")] public FullSyncStatus SyncStatus { get; set; }

        [JsonPropertyName("audit")] public ActionHistory Audit { get; set; }
    }
}