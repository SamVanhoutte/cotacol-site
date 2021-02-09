using System.Collections.Generic;
using System.Threading.Tasks;
using CotacolApp.Models;
using CotacolApp.Models.CotacolApi;

namespace CotacolApp.Interfaces
{
    public interface ICotacolUserClient
    {
        Task<List<UserColAchievement>> GetColsAsync();
        Task<List<UserClimb>> GetClimbDataAsync();

        Task<List<CotacolActivity>> GetActivitiesAsync();
        Task<UserAchievements> GetAchievementsAsync(bool includeLocalLegends = false);
        Task<UserProfile> GetProfileAsync();
        Task<SyncStatus> GetSyncStatus();
        Task<int> SynchronizeAsync( bool fullSync = false);
        Task SetUserPermissionsAsync(string permissionScope);
    }
}