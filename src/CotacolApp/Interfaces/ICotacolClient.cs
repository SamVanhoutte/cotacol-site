using System.Collections.Generic;
using System.Threading.Tasks;
using CotacolApp.Models;
using CotacolApp.Models.CotacolApi;

namespace CotacolApp.Interfaces
{
    public interface ICotacolClient
    {
        Task<List<ClimbData>> GetClimbDataAsync();
        Task<SiteStats> GetStatsAsync();
        Task<bool> SetupUserAsync(UserSetupRequest userSettings);
    }
}   