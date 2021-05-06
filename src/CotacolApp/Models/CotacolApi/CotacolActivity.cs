using System;

namespace CotacolApp.Models.CotacolApi
{
    public record CotacolActivity
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ActivityId { get; set; }
        public DateTime ActivityDate { get; set; }
        public int UniqueCols { get; set; }
        public int TotalPoints { get; set; }
        public string ActivityName { get; set; }
        public double Distance { get; set; }
        public double Elevation { get; set; }
        public DateTime SyncTime { get; set; }
        public string ActivityUrl => $"https://www.strava.com/activities/{ActivityId}";

    }
}