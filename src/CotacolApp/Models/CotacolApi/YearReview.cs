using System;
using System.Collections.Generic;

namespace CotacolApp.Models.CotacolApi
{
    public class YearReview
    {
        public string UserName { get; set; }
        public int MostPopularColCount { get; set; }
        public int UniqueColsInYear { get; set; }
        public int PointsInYear { get; set; }
        public long ElevationInYear { get; set; }
        public long DistanceInYear { get; set; }
        public int TotalCols { get; set; }
        public int TotalPoints { get; set; }
        public long TotalElevation { get; set; }
        public long TotalLength { get; set; }
        public CotacolClimbTrackRecord MostPopularCol { get; set; }
    }
    
    public class CotacolClimbTrackRecord
    {
        public string CotacolId { get; set; }
        public string CotacolName { get; set; }
        public int CotacolPoints { get; set; }
        public List<ColAchievement> Achievements { get; set; }
    }
    
    public class ColAchievement
    {
        public string ActivityId { get; set; }
        public DateTime AchievementDate { get; set; }
        public TimeSpan Duration { get; set; }
    }
}