using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
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
        [JsonProperty("activityId")]
        public string ActivityId { get; set; }
        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
}