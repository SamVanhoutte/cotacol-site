using System.Collections.Generic;
using System.Threading.Tasks;
using CotacolApp.Models.CotacolApi;

namespace CotacolApp.Interfaces
{
    public interface ICotacolClient
    {
        Task<List<Climb>> GetClimbDataAsync();
        Task<SiteStats> GetStatsAsync();
        Task<bool> SetupUserAsync(UserSetupRequest userSettings);
    }
}   