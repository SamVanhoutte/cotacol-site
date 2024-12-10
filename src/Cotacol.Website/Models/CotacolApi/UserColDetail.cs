using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    public partial class UserColDetail
    {
        [JsonPropertyName("activities")]
        public List<ColActivity> Activities { get; set; }

        [JsonPropertyName("bookmarked")]
        public bool Bookmarked { get; set; }
    }
}