using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CotacolApp.Interfaces;
using CotacolApp.Models;
using CotacolApp.Models.CotacolApi;
using CotacolApp.Settings;
using Flurl.Http;
using GuardNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CotacolApp.Services
{
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

        private string userId => _context.HttpContext.User.GetUserId();

        public async Task<List<UserClimb>> GetClimbDataAsync()
        {
            var achievements = await GetAchievementsAsync();
            var allClimbs =
                (await _cotacolDataClient
                    .GetClimbDataAsync())
                .Select(climb => new UserClimb(climb));

            var stats =
                (await _cotacolDataClient
                    .GetStatsAsync());

            _logger.LogInformation($"User {userId} has {achievements.ColResults.Count()} conquered climbs");
            var result = new List<UserClimb>();
            foreach (var climb in allClimbs)
            {
                // First check users results for the climb
                var achievement = achievements.ColResults.FirstOrDefault(c => c.CotacolId.Equals(climb.Id));
                climb.Done = achievement != null;
                if (achievement != null)
                {
                    climb.FirstAchievement = achievement.Achievements.OrderBy(a => a.AchievementDate).FirstOrDefault()
                        ?.AchievementDate ?? default;
                    climb.Attempts = achievement.Achievements.Count;
                    climb.Duration = achievement.Achievements.Min(a => TimeSpan.Parse(a.Duration).TotalSeconds);
                }

                // Now update with the stats
                // TODO
                result.Add(climb);
            }

            _logger.LogInformation($"Returned cotacols with {result.Count(c => c.Done)} marked climbs");
            return result;
        }

        public async Task<List<UserColAchievement>> GetColsAsync()
        {
            var cols = await $"{_settings.ApiUrl}/user/{userId}/cols"
                .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
                .GetJsonAsync<List<UserColAchievement>>();

            return cols;
        }

        public Task<List<CotacolActivity>> GetActivitiesAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<UserAchievements> GetAchievementsAsync(bool includeLocalLegends = false)
        {
            var achievements = await $"{_settings.ApiUrl}/user/{userId}/achievements"
                .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
                .GetJsonAsync<UserAchievements>();

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

        public Task<UserProfile> GetProfileAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<SyncStatus> GetSyncStatus()
        {
            var response = await $"{_settings.ApiUrl}/user/{userId}/sync"
                .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
                .GetJsonAsync<SyncStatus>();
            return response;
        }

        public async Task<int> SynchronizeAsync(bool fullSync = false)
        {
            _logger.LogInformation($"Sync request for user {userId}");
            var response = await $"{_settings.ApiUrl}/user/{userId}/sync"
                .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
                .AllowAnyHttpStatus()
                .PostJsonAsync(new SyncRequest {ForceSync = false, FullSync = fullSync, MaxActivityCount = 0});
            _logger.LogInformation($"Sync requested for user {userId} with status {response.StatusCode}");
            return response.StatusCode;
        }
    }
}