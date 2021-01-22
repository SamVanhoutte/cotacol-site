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

        Task<List<CotacolActivity>> GetActivitiesAsync(string userId);
        Task<UserAchievements> GetAchievementsAsync(string userId);
        Task<UserProfile> GetProfileAsync(string userId);
        Task<SyncStatus> GetSyncStatus(string userId);
    }
}