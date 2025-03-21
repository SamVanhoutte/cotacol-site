using Cotacol.Website.Models;
using Cotacol.Website.Models.CotacolApi;

namespace Cotacol.Website.Interfaces
{
    public interface ICotacolClient
    {
        Task<List<ClimbData>> GetClimbDataAsync();
        Task<List<ClimbData>> GetComparableClimbsAsync(string cotacolId, int count);
        Task<SiteStats> GetStatsAsync();
        Task<bool> SetupUserAsync(UserSetupRequest userSettings);
        Task<HomeStats> GetHomeStatsAsync();
        Task<ClimbDetail> GetClimbSegmentsAsync(string cotacolId);
        Task<ClimbUserDetail> GetClimbDetailAsync(string cotacolId, string userId);
        Task<StravaSegment> FetchStravaSegmentAsync(string stravaSegmentId, bool persistMetadata = false);
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
        Task<List<SupportCase>> GetSupportCasesAsync(); 
        Task<SupportCase> CreateSupportCaseAsync(SupportCaseRequest request); 
        Task UpdateSupportCaseAsync(string caseId, SupportCaseUpdate request);
        Task<SupportCase> GetSupportCaseAsync(string caseId);
        Task AddSupportCaseMessageAsync(string caseId, SupportChat message);
    }
}   