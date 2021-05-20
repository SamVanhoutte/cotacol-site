using System.Collections.Generic;
using Newtonsoft.Json;

namespace CotacolApp.Models.CotacolApi
{
    public partial class UserColDetail
    {
        [JsonProperty("activities")]
        public List<ColActivity> Activities { get; set; }

        [JsonProperty("bookmarked")]
        public bool Bookmarked { get; set; }
    }
}