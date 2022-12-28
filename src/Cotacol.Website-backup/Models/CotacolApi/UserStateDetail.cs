using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cotacol.Website.Models.CotacolApi
{
    public class UserStateDetail
    {
            [JsonProperty("tokens")]
            public UserTokens UserTokens { get; set; }

            [JsonProperty("state")]
            public JObject State { get; set; }
            
            [JsonProperty("syncStatus")]
            public FullSyncStatus SyncStatus { get; set; }
            
            [JsonProperty("audit")]
            public ActionHistory Audit { get; set; }
    }
}