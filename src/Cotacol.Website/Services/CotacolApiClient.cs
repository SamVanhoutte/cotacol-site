using Cotacol.Website.Interfaces;
using Cotacol.Website.Models;
using Cotacol.Website.Models.CotacolApi;
using Cotacol.Website.Models.Settings;
using Flurl;
using Flurl.Http;
using GuardNet;
using Microsoft.Extensions.Options;

namespace Cotacol.Website.Services;

public class CotacolApiClient : ICotacolClient
{
    private CotacolApiSettings _settings;
    private ILogger<CotacolApiClient> _logger;
    private List<ClimbData>? _climbData;

    public CotacolApiClient(IOptions<CotacolApiSettings> apiSettings, ILogger<CotacolApiClient> logger)
    {
        Guard.NotNull(apiSettings?.Value, nameof(apiSettings));
        _logger = logger;
        _settings = apiSettings.Value;
    }

    public async Task<List<ClimbData>> GetClimbDataAsync()
    {
        if (_climbData == null)
        {
            _climbData = await $"{_settings.ApiUrl}/cotacoldata"
                .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
                .GetJsonAsync<List<ClimbData>>();
        }

        return _climbData;
    }

    public async Task<List<ClimbData>> GetComparableClimbsAsync(string cotacolId, int count)
    {
        var climbs = await GetClimbDataAsync();
        var compareCol = climbs.First(c => c.Id.Equals(cotacolId));
        var rnd = new Random();
        return climbs
            .Where(c => c.Id != cotacolId)
            .OrderBy(col => CompareScore(col, compareCol)).Take(count).ToList();
    }

    private double CompareScore(ClimbData referenceCol, ClimbData compareCol)
    {
        // Distance : 100 - 13500
        var distanceOff = getComparePercentage(referenceCol.Distance, compareCol.Distance, 100, 13500);
        // Grade : 3,3 - 20,5
        var gradientOff = getComparePercentage(referenceCol.AvgGrade, compareCol.AvgGrade, 3.3, 20.5);
        // Elevation : 17 - 449
        var altitudeOff = getComparePercentage(referenceCol.ElevationDiff, compareCol.ElevationDiff, 17, 449);
        // Points : 41 - 363
        var scoreOff = getComparePercentage(referenceCol.CotacolPoints, compareCol.CotacolPoints, 41, 363);
        var score = distanceOff + gradientOff + altitudeOff + scoreOff;
        return score;
    }

    private double getComparePercentage(double compValue, double refValue, double minValue, double maxValue)
    {
        var value1 = (compValue - minValue) / (maxValue - minValue);
        var value2 = (refValue - minValue) / (maxValue - minValue);
        return Math.Abs(value1 - value2);
    }

    public async Task<SiteStats> GetStatsAsync()
    {
        var stats = await $"{_settings.ApiUrl}/stats"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .GetJsonAsync<SiteStats>();
        var idx = 1;
        foreach (var statsUser in stats.Users)
        {
            statsUser.Position = idx++;
        }

        return stats;
    }

    public async Task<bool> SetupUserAsync(UserSetupRequest userSettings)
    {
        // TODO : validation of required props
        var response = await $"{_settings.ApiUrl}/user/{userSettings.UserId}"
            .AllowAnyHttpStatus()
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .PostJsonAsync(userSettings);
        var bodyResponse = await response.ResponseMessage.Content.ReadAsStringAsync();
        _logger.LogInformation($"User setup result ({response.StatusCode}) with content {bodyResponse}");
        return response.ResponseMessage.IsSuccessStatusCode;
    }

    public async Task<HomeStats> GetHomeStatsAsync()
    {
        var stats = await $"{_settings.ApiUrl}/stats/home"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .GetJsonAsync<HomeStats>();

        return stats;
    }

    public async Task<ClimbDetail> GetClimbSegmentsAsync(string cotacolId)
    {
        var segmentData = await $"{_settings.ApiUrl}/segments/{cotacolId}"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .GetJsonAsync<ClimbDetail>();

        return segmentData;
    }

    public async Task<ClimbUserDetail> GetClimbDetailAsync(string cotacolId, string userId)
    {
        var url = string.IsNullOrEmpty(userId)
            ? $"{_settings.ApiUrl}/climbs/{cotacolId}"
            : $"{_settings.ApiUrl}/users/{userId}/climbs/{cotacolId}";
        try
        {
            var cotacolDetail = await url
                .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
                .GetJsonAsync<ClimbUserDetail>();
            return cotacolDetail;
        }
        catch (Exception e)
        {
            throw;
        }

    }

    public async Task<StravaSegment> FetchStravaSegmentAsync(string stravaSegmentId,
        bool persistMetadata = false)
    {
        if (stravaSegmentId.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
        {
            stravaSegmentId = stravaSegmentId.Split('/').Last();
        }

        var segmentResponse = await $"{_settings.ApiUrl}/segments/strava/{stravaSegmentId}"
            .SetQueryParam("UpdateMetadata", persistMetadata)
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .GetJsonAsync<StravaSegment>();

        return segmentResponse;
    }

    public async Task<int> UpdateSegmentAsync(string cotacolId, UpdateSegmentRequest update)
    {
        var response = await $"{_settings.ApiUrl}/climbs/{cotacolId}"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .PostJsonAsync(update);
        return response.StatusCode;
    }

    public async Task<List<UserRecord>> GetUsersAsync(bool loadTokens, bool loadSyncStatus)
    {
        return await $"{_settings.ApiUrl}/users"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .SetQueryParam("loadtokens", loadTokens)
            .SetQueryParam("loadsyncstatus", loadSyncStatus)
            .GetJsonAsync<List<UserRecord>>();
    }

    public async Task<List<UserListRecord>> GetUserListAsync()
    {
        return await $"{_settings.ApiUrl}/users"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .GetJsonAsync<List<UserListRecord>>();
    }

    public async Task<List<SegmentDataValidation>> GetSegmentListAsync()
    {
        return await $"{_settings.ApiUrl}/segments"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .GetJsonAsync<List<SegmentDataValidation>>();
    }

    public async Task<UserStateDetail> GetUserAdminInfoAsync(string userId)
    {
        var userState = await $"{_settings.ApiUrl}/user/{userId}/state"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .GetJsonAsync<UserStateDetail>();

        return userState;
    }

    public async Task<int> UpdateCotacolMetadataAsync(string cotacolId, UpdateSegmentRequest update)
    {
        var response = await $"{_settings.ApiUrl}/climbs/{cotacolId}"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .PutJsonAsync(update);

        return response.StatusCode;
    }

    public async Task<IEnumerable<BadgeOfMonthData>> GetBadgeOfMonthListAsync()
    {
        var response = await $"{_settings.ApiUrl}/system/badges/month"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .GetJsonAsync<List<BadgeOfMonthData>>();
        return response.OrderByDescending(b => b.MonthKey);
    }

    public async Task UpdateBadgeOfMonthAsync(BadgeOfMonthData badgeOfMonthData)
    {
        var response = await $"{_settings.ApiUrl}/system/badges/month"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .PutJsonAsync(badgeOfMonthData);
    }

    public async Task DeleteBadgeOfMonthAsync(int year, int month)
    {
        var response = await $"{_settings.ApiUrl}/system/badges/month/{year}/{month}"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .DeleteAsync();
    }

    public async Task<SystemStatus> GetSystemStatusAsync()
    {
        var systemStatus = await $"{_settings.ApiUrl}/system/status"
            .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
            .GetJsonAsync<SystemStatus>();

        return systemStatus;
    }
}