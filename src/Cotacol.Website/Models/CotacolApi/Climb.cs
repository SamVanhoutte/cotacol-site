using Newtonsoft.Json;

namespace Cotacol.Website.Models.CotacolApi
{
    
    public class Climb
    {
        public string Id { get; init; }
        [JsonProperty("cotacol_points")]
        public int CotacolPoints { get; init; }
        public string Province { get; init; }
        public string Url { get; init; }
        public string Surface { get; init; }
        [JsonProperty("segment_confidence")]
                public string SegmentConfidence { get; init; }
        public string Name { get; init; }

        // public  UserClimb ToUserClimb()
        // {
        //     return new()
        //     {
        //         Id = Id, Name = Name, CotacolPoints = CotacolPoints, Province = Province,
        //         Surface = Surface, SegmentConfidence = SegmentConfidence, Url = Url
        //     };
        // }
    }
    

}