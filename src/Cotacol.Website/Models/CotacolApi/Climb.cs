using System.Text.Json.Serialization;


namespace Cotacol.Website.Models.CotacolApi
{
    
    public class Climb
    {
        public string Id { get; init; }
        [JsonPropertyName("cotacol_points")]
        public int CotacolPoints { get; init; }
        public string Province { get; init; }
        public string Url { get; init; }
        public string Surface { get; init; }
        [JsonPropertyName("segment_confidence")]
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