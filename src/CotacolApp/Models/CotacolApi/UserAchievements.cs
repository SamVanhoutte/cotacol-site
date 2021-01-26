using System.Collections.Generic;

namespace CotacolApp.Models.CotacolApi
{
    public record UserAchievements    {
        public string UserId { get; set; } 
        public string UserName { get; set; } 
        public string FullName { get; set; } 
        public int TotalScore { get; set; } 
        public int UniqueCols { get; set; } 
        public int TotalAscent { get; set; } 
        public int TotalLength { get; set; } 
        public List<ColResult> ColResults { get; set; } 
    }
}