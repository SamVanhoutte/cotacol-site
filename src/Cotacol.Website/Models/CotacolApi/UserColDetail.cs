using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public partial class UserColDetail
    {
        [JsonProperty("activities")]
        public List<ColActivity> Activities { get; set; }

        [JsonProperty("bookmarked")]
        public bool Bookmarked { get; set; }
    }
}