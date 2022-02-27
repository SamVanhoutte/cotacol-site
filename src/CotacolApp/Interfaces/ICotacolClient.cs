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
        Task<HomeStats> GetHomeStatsAsync();
        Task<ClimbDetail> GetClimbSegmentsAsync(string cotacolId);
        Task<ClimbUserDetail> GetClimbDetailAsync(string cotacolId, string userId);
        Task<StravaSegmentResponse> FetchStravaSegmentAsync(string stravaSegmentId, bool persistMetadata = false);
        Task<int> UpdateSegmentAsync(string cotacolId, UpdateSegmentRequest update);
        Task<List<UserRecord>> GetUsersAsync(bool loadTokens, bool loadSyncStatus);
        Task<List<UserListRecord>> GetUserListAsync();
        
        Task<List<SegmentDataValidation>> GetSegmentListAsync();
        Task<UserStateDetail> GetUserAdminInfoAsync(string userId);
        Task<int> UpdateCotacolMetadataAsync(string cotacolId, UpdateSegmentRequest updateRequest);

        Task<IEnumerable<BadgeOfMonthData>> GetBadgeOfMonthListAsync();
        Task UpdateBadgeOfMonthAsync(BadgeOfMonthData badgeOfMonthData);
        Task DeleteBadgeOfMonthAsync(int year, int month);
        Task<SystemStatus> GetSystemStatusAsync();
    }
}   