using System.Collections.Generic;

namespace CotacolApp.Models.CotacolApi
{
    public class SiteStats    {
        public int TotalPoints { get; set; } 
        public int UniqueCols { get; set; } 
        public List<User> Users { get; set; } 
        public List<Cotacol> Cotacols { get; set; } 
        public List<Activity> Activities { get; set; } 
    }
}