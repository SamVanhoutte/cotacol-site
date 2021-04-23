using System;

namespace CotacolApp.Models
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
        }
        
        public bool Done { get; set; }
        public DateTime FirstAchievement { get; set; }
        public int Attempts { get; set; }
        public double Duration { get; set; }
    }
}