namespace Cotacol.Website.Models
{
    public class UserClimb : ClimbData
    {
        public UserClimb()
        {
        }

        public UserClimb(ClimbData metaData)
        {
            Name = metaData.Name;
            City = metaData.City;
            Province = metaData.Province;
            CotacolPoints = metaData.CotacolPoints;
            Distance = metaData.Distance;
            ElevationDiff = metaData.ElevationDiff;
            AvgGrade = metaData.AvgGrade;
            Surface = metaData.Surface;
            Aliases = metaData.Aliases;
            StravaSegment = metaData.StravaSegment;
            Id = metaData.Id;
            Url = metaData.Url;
            BomScore = metaData.BomScore;
            Polyline = metaData.Polyline;
            UniqueUsers = metaData.UniqueUsers;
            TotalAttempts = metaData.TotalAttempts;
            LocalLegends = metaData.LocalLegends;
        }
        
        public bool Done { get; set; }
        public DateTime FirstAchievement { get; set; }
        public int UserAttempts { get; set; }
        public double BestTime { get; set; }
        public bool Bookmarked { get; set; }
        public bool LocalLegend { get; set; }
        public int AttemptsNeededToLegend => CurrentLegendLimit - UserAttempts;

    }
}