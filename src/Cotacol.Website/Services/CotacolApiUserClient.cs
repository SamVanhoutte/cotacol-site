using Cotacol.Website.Interfaces;
using Cotacol.Website.Models;
using Cotacol.Website.Models.CotacolApi;
using Cotacol.Website.Models.Settings;
using Cotacol.Website.Services.Extensions;

namespace Cotacol.Website.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using GuardNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class CotacolApiUserClient : ICotacolUserClient
{
    private CotacolApiSettings _settings;
    private IHttpContextAccessor _context;
    private ICotacolClient _cotacolDataClient;
    private ILogger<CotacolApiUserClient> _logger;

    public CotacolApiUserClient(IHttpContextAccessor contextAccessor, ICotacolClient cotacolDataClient,
        ILogger<CotacolApiUserClient> logger, IOptions<CotacolApiSettings> settings)
    {
        Guard.NotNull(settings?.Value, nameof(settings));
        _context = contextAccessor;
        _cotacolDataClient = cotacolDataClient;
        _logger = logger;
        _settings = settings.Value;
    }

    private string currentUserId => _context.HttpContext.User.GetUserId();

    public async Task<List<UserClimb>> GetClimbDataAsync(string userId)
    {
        var userClimbs = await $"{_settings.ApiUrl}/cotacoldata/{userId}"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .GetJsonAsync<List<UserClimb>>();

        _logger.LogInformation($"Returned cotacols with {userClimbs.Count(c => c.Done)} marked climbs");
        return userClimbs;
    }

    public async Task<List<UserColAchievement>> GetColsAsync(string userId)
    {
        var cols = await $"{_settings.ApiUrl}/user/{userId}/cols"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .GetJsonAsync<List<UserColAchievement>>();

        return cols;
    }

    public async Task<List<CotacolActivity>> GetActivitiesAsync(string userId = null, bool allActivities = true)
    {
        userId ??= currentUserId;
        var cols = await $"{_settings.ApiUrl}/user/{userId}/activities"
            .SetQueryParam("allactivities", allActivities)
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .GetJsonAsync<List<CotacolActivity>>();

        return cols;
    }

    public async Task<bool> GetBookmarkClimbAsync(string climbId, string userId = null)
    {
        userId ??= currentUserId;

        var isBookmarked = await $"{_settings.ApiUrl}/user/{userId}/bookmark/{climbId}"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .GetJsonAsync<bool>();

        return isBookmarked;
    }

    public async Task<bool> BookmarkClimbAsync(string climbId, string userId = null)
    {
        userId ??= currentUserId;

        var response = await $"{_settings.ApiUrl}/user/{userId}/bookmark/{climbId}"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .PostAsync();
        return response.StatusCode >= 200 && response.StatusCode < 299;
    }

    public async Task<bool> UnbookmarkClimbAsync(string climbId, string userId = null)
    {
        userId ??= currentUserId;

        var response = await $"{_settings.ApiUrl}/user/{userId}/bookmark/{climbId}"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .DeleteAsync();
        return response.StatusCode >= 200 && response.StatusCode < 299;
    }

    public async Task<UserAchievements> GetAchievementsAsync(string userId, bool includeLocalLegends = false)
    {
        try
        {
            var achievements = await $"{_settings.ApiUrl}/user/{userId}/achievements"
                .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
                .AllowAnyHttpStatus()
                .GetJsonAsync<UserAchievements>();
            achievements ??= new UserAchievements();
            if (includeLocalLegends)
            {
                achievements.LocalLegends = new Dictionary<string, int>();
                var stats = await _cotacolDataClient.GetStatsAsync();
                foreach (var colStats in stats.Cotacols.Where(col =>
                             col.LocalLegends != null &&
                             col.LocalLegends.Any(leg => leg.UserId.Equals(userId))))
                {
                    achievements.LocalLegends.Add(colStats.CotacolId, colStats.LocalLegends.First().Attempts);
                }
            }

            return achievements;
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, $"Achievement retrieval failed with error {e.Message}");
            throw;
        }
    }

    public async Task<UserProfile> GetProfileAsync(string userId = null)
    {
        userId ??= currentUserId;
        var response = await $"{_settings.ApiUrl}/user/{userId}"
            .AllowAnyHttpStatus()
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue).GetAsync();

        if (response.StatusCode >= 200 && response.StatusCode < 300)
        {
            return await response.ResponseMessage.Content.ReadFromJsonAsync<UserProfile>();
        }
        return null;
    }

    public async Task<StravaUserProfile> GetStravaUserConfigurationAsync(string userId = null)
    {
        userId ??= currentUserId;
        var profile = await $"{_settings.ApiUrl}/user/strava/{userId}"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .GetJsonAsync<StravaUserProfile>();

        return profile;
    }

    public async Task UpdateSettingsAsync(string userId, UserSettings settings, string email)
    {
        var req = new UserSetupRequest
        {
            UserId = userId,
            UpdateActivityDescription = settings.UpdateActivityDescription,
            CotacolHunter = settings.CotacolHunter,
            EnableBetaFeatures = settings.EnableBetaFeatures,
            PrivateProfile = settings.PrivateProfile
        };
        if (!string.IsNullOrEmpty(settings?.PersistenceService))
        {
            req.PersistenceService = settings.PersistenceService;
        }

        if (!string.IsNullOrEmpty(email))
        {
            req.EmailAddress = email;
        }

        await _cotacolDataClient.SetupUserAsync(req);
    }

    public async Task<SyncStatus> GetSyncStatus(string userId)
    {
        var response = await $"{_settings.ApiUrl}/user/{userId}/sync"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .GetJsonAsync<SyncStatus>();
        return response;
    }

    public async Task<AsyncWorkflowResult> SynchronizeAsync(string userId, bool fullSync = false)
    {
        _logger.LogInformation($"Sync request for user {userId}");
        var response = await $"{_settings.ApiUrl}/user/{userId}/sync"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .AllowAnyHttpStatus()
            .PostJsonAsync(new SyncRequest {ForceSync = false, FullSync = fullSync, MaxActivityCount = 0});
        _logger.LogInformation($"Sync requested for user {userId} with status {response.StatusCode}");
        var result = new AsyncWorkflowResult();
        if (response.StatusCode >= 200 && response.StatusCode < 300)
        {
            result = await response.ResponseMessage.Content.ReadFromJsonAsync<AsyncWorkflowResult>();
        }

        result.HttpStatus = response.StatusCode;
        return result;
    }

    public async Task<AsyncWorkflowResult> SynchronizeActivityAsync(string userId, string activityId)
    {
        _logger.LogInformation($"Sync request for user {userId} activity {activityId}");
        var response = await $"{_settings.ApiUrl}/user/{userId}/activity/{activityId}/sync"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .AllowAnyHttpStatus()
            .PostJsonAsync(new { });
        _logger.LogInformation($"Sync requested for activity {activityId} with status {response.StatusCode}");
        var result = new AsyncWorkflowResult();
        if (response.StatusCode >= 200 && response.StatusCode < 300)
        {
            result = await response.ResponseMessage.Content.ReadFromJsonAsync<AsyncWorkflowResult>();
        }

        result.HttpStatus = response.StatusCode;
        return result;
    }

    public async Task<int> SubmitMissingSegmentAsync(string missingActivityId, string missingCotacolId,
        string remark = "")
    {
        _logger.LogInformation(
            $"Removing user missing segment for user {currentUserId}: col : {missingCotacolId} : activity : {missingActivityId}");
        var response = await $"{_settings.ApiUrl}/data/missing"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .AllowAnyHttpStatus()
            .PostJsonAsync(new MissingDataRequest
            {
                UserId = currentUserId,
                ActivityId = missingActivityId,
                CotacolId = missingCotacolId,
                Remark = remark
            });
        _logger.LogInformation($"Missing col published for user {currentUserId} with status {response.StatusCode}");
        return response.StatusCode;
    }

    public async Task<int> RemoveUserAsync(string userId)
    {
        _logger.LogWarning($"Removing user {userId} from the Backend, executed by admin {currentUserId}");
        var response = await $"{_settings.ApiUrl}/user/{userId}"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .AllowAnyHttpStatus()
            .DeleteAsync();
        _logger.LogInformation($"User delete requested for user {userId} with status {response.StatusCode}");
        return response.StatusCode;
    }

    public async Task<YearReview> GetYearReviewAsync(string userId, int year)
    {
        var response = await $"{_settings.ApiUrl}/user/{userId}/year"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .GetJsonAsync<YearReview>();
        return response;
    }

    public async Task<List<UserBadgeStatus>> GetBadgesAsync(string userId)
    {
        var response = await $"{_settings.ApiUrl}/user/{userId}/badges"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .GetJsonAsync<List<UserBadgeStatus>>();
        return response;
    }

    public async Task<UserBadgeStatus> GetBadgeAsync(string badgeId, string userId)
    {
        var response = await $"{_settings.ApiUrl}/user/{userId}/badges/{badgeId}"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .GetJsonAsync<UserBadgeStatus>();
        return response;
    }

    public async Task<List<BadgeSyncResult>> SynchronizeUserBadgesAsync(string userId)
    {
        _logger.LogInformation($"Sync request for user {userId} badges");
        var response = await $"{_settings.ApiUrl}/user/{userId}/badges/sync"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .AllowAnyHttpStatus()
            .PostJsonAsync(new { });
        _logger.LogInformation($"Sync requested for badges of user {userId} with status {response.StatusCode}");
        var result = new List<BadgeSyncResult>();
        if (response.StatusCode >= 200 && response.StatusCode < 300)
        {
            result = await response.ResponseMessage.Content.ReadFromJsonAsync<List<BadgeSyncResult>>();
        }

        return result;
    }
}