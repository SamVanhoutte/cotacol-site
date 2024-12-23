using System.Text.Json.Serialization;

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