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
using SQLitePCL;

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
            var colsDone = await GetColsAsync();
            var allClimbs = (await _cotacolDataClient.GetClimbDataAsync()).Select(climb => climb.ToUserClimb());

            _logger.LogInformation($"User {userId} has {colsDone.Count()} conquered climbs");
            var result = new List<UserClimb>();
            foreach (var climb in allClimbs)
            {
                var achievement = colsDone.FirstOrDefault(c => c.CotacolId.Equals(climb.Id));
                climb.Done = achievement != null;
                if (achievement != null)
                {
                    climb.FirstAchievement = achievement.AchievementDate;
                }

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

        public Task<UserAchievements> GetAchievementsAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<UserProfile> GetProfileAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<SyncStatus> GetSyncStatus()
        {
            var response = await $"{_settings.ApiUrl}/user/{userId}/sync"
                .WithHeader(_settings.SharedKeyHeaderName,  _settings.SharedKeyValue)
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