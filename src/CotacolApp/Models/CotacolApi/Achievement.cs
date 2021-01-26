using System;

namespace CotacolApp.Models.CotacolApi
{
    public record Achievement    {
        public string ActivityId { get; set; } 
        public DateTime AchievementDate { get; set; } 
        public string Duration { get; set; } 
        public int MatchingType { get; set; } 
    }
}