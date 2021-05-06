using System;

namespace CotacolApp.Models.CotacolApi
{
    public class Activity    {
        public string ActivityId { get; set; } 
        public string UserId { get; set; } 
        public string UserName { get; set; } 
        public string FullName { get; set; } 
        public DateTime ActivityDate { get; set; } 
        public int UniqueCols { get; set; } 
        public int TotalPoints { get; set; } 
        public string ActivityUrl => $"https://www.strava.com/activities/{ActivityId}";
 
    }
}