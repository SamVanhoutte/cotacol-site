using System;
using CotacolApp.Models.CotacolApi;

namespace CotacolApp.Models
{
    public record UserClimb : Climb
    {
        public UserClimb():base()
        {
            
        }
        public bool Done { get; set; }
        public DateTime FirstAchievement { get; set; }
        public int Attempts { get; set; }
        public int Duration { get; set; }
    }
}