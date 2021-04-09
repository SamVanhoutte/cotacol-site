using System.Collections.Generic;
using System.Linq;
using Microsoft.ApplicationInsights;

namespace CotacolApp.Models.CotacolApi
{
    public class SiteStats    
    {
        public int TotalPoints { get; set; } 
        public int UniqueCols { get; set; } 
        public List<UserStatistics> Users { get; set; } 
        public List<Cotacol> Cotacols { get; set; } 
        public List<Activity> Activities { get; set; }

        public void SortUsers(bool byPoints)
        {
            var idx = 0;
            Users = Users.OrderByDescending((us) => byPoints ? us.TotalPoints : us.UniqueCols).ToList();
            foreach (var user in Users)
            {
                user.Position = ++idx;
            }
        }
    }
}