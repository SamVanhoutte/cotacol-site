using System;

namespace CotacolApp.Models.CotacolApi
{
    public record UserColAchievement
    {
        public string CotacolId { get; set; } 
        public DateTime AchievementDate { get; set; } 
    }
}