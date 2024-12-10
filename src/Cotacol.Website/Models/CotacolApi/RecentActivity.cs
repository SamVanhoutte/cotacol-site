using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public class RecentActivity
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("cotacolId")]
        public string CotacolId { get; set; }

        [JsonPropertyName("cotacolName")]
        public string CotacolName { get; set; }

        [JsonPropertyName("activityDate")]
        public DateTime ActivityDate { get; set; }

        [JsonPropertyName("fullName")]
        public string FullName { get; set; }
        [JsonPropertyName("activityId")]
        public string ActivityId { get; set; }
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
    }
}