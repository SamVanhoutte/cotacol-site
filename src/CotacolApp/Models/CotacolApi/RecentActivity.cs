using System;
using Newtonsoft.Json;

namespace CotacolApp.Models.CotacolApi
{
    public class RecentActivity
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("cotacolId")]
        public string CotacolId { get; set; }

        [JsonProperty("cotacolName")]
        public string CotacolName { get; set; }

        [JsonProperty("activityDate")]
        public DateTime ActivityDate { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }
    }
}