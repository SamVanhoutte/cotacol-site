using System.Collections.Generic;
using System.Threading.Tasks;
using CotacolApp.Models;
using CotacolApp.Models.CotacolApi;

namespace CotacolApp.Interfaces
{
    public interface ICotacolUserClient
    {
        Task<List<UserColAchievement>> GetColsAsync(string userId);
        Task<List<UserClimb>> GetClimbDataAsync(string userId);
        Task<List<CotacolActivity>> GetActivitiesAsync();
        Task<UserAchievements> GetAchievementsAsync(string userId, bool includeLocalLegends = false);
        Task<UserProfile> GetProfileAsync(string userId=null);
        Task UpdateSettingsAsync(UserSettings settings);
        Task<SyncStatus> GetSyncStatus(string userId);
        Task<int> SynchronizeAsync(string userId, bool fullSync = false);
        Task<int> SubmitMissingSegmentAsync(string missingActivityId, string missingCotacolId, string remark = "");
    }
}