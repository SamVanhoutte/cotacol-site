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
        Task<List<CotacolActivity>> GetActivitiesAsync(string userId = null, bool allActivities = false);
        Task<bool> GetBookmarkClimbAsync(string climbId, string userId = null);
        Task<bool> BookmarkClimbAsync(string climbId, string userId = null);
        Task<bool> UnbookmarkClimbAsync(string climbId, string userId = null);
        Task<UserAchievements> GetAchievementsAsync(string userId, bool includeLocalLegends = false);
        Task<UserProfile> GetProfileAsync(string userId=null);
        Task<StravaUserProfile> GetStravaUserConfigurationAsync(string userId=null);
        Task UpdateSettingsAsync(UserSettings settings, string emailAddress);
        Task<SyncStatus> GetSyncStatus(string userId);
        Task<AsyncWorkflowResult> SynchronizeAsync(string userId, bool fullSync = false);
        Task<AsyncWorkflowResult> SynchronizeActivityAsync(string userId, string activityId);
        Task<int> SubmitMissingSegmentAsync(string missingActivityId, string missingCotacolId, string remark = "");
        Task<int> RemoveUserAsync(string userId);
        Task<YearReview> GetYearReviewAsync(string userId, int year);
        Task<List<UserBadgeStatus>> GetBadgesAsync(string userId);
        Task<UserBadgeStatus> GetBadgeAsync(string badgeId, string userId);
        Task<List<BadgeSyncResult>> SynchronizeUserBadgesAsync(string userId);
    }
}