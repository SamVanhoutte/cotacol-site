using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public  class AsyncWorkflowResult
    {
        [JsonPropertyName("status")]
        public int HttpStatus { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("statusQueryGetUri")]
        public string StatusQueryGetUri { get; set; }

        [JsonPropertyName("sendEventPostUri")]
        public string SendEventPostUri { get; set; }

        [JsonPropertyName("terminatePostUri")]
        public string TerminatePostUri { get; set; }

        [JsonPropertyName("purgeHistoryDeleteUri")]
        public string PurgeHistoryDeleteUri { get; set; }

        [JsonPropertyName("restartPostUri")]
        public string RestartPostUri { get; set; }
    }
}