using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public  class AsyncWorkflowResult
    {
        [JsonProperty("status")]
        public int HttpStatus { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("statusQueryGetUri")]
        public string StatusQueryGetUri { get; set; }

        [JsonProperty("sendEventPostUri")]
        public string SendEventPostUri { get; set; }

        [JsonProperty("terminatePostUri")]
        public string TerminatePostUri { get; set; }

        [JsonProperty("purgeHistoryDeleteUri")]
        public string PurgeHistoryDeleteUri { get; set; }

        [JsonProperty("restartPostUri")]
        public string RestartPostUri { get; set; }
    }
}