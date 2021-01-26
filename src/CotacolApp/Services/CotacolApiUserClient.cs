using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CotacolApp.Interfaces;
using CotacolApp.Models;
using CotacolApp.Models.CotacolApi;
using CotacolApp.Settings;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
            ILogger<CotacolApiUserClient> logger)
        {
            // TODO : options
            _settings = new CotacolApiSettings
            {
                BaseUrl = "https://cotacol-hunting.azure-api.net/test/api/v1", SecretKeyName = "x-api-subscription-key"
            };
            _context = contextAccessor;
            _cotacolDataClient = cotacolDataClient;
            _logger = logger;
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
            var cols = await $"{_settings.BaseUrl}/user/{userId}/cols"
                .WithHeader(_settings.SecretKeyName, "8963ff1421164023ac3d789567a58896")
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
            var response = await $"{_settings.BaseUrl}/user/{userId}/sync"
                .WithHeader(_settings.SecretKeyName, "8963ff1421164023ac3d789567a58896")
                .GetJsonAsync<SyncStatus>();
            return response;
        }

        public async Task<int> SynchronizeAsync(bool fullSync = false)
        {
            _logger.LogInformation($"Sync request for user {userId}");
            var response = await $"{_settings.BaseUrl}/user/{userId}/sync"
                .WithHeader(_settings.SecretKeyName, "8963ff1421164023ac3d789567a58896")
                .AllowAnyHttpStatus()
                .PostJsonAsync(new SyncRequest {ForceSync = false, FullSync = fullSync, MaxActivityCount = 0});
            _logger.LogInformation($"Sync requested for user {userId} with status {response.StatusCode}");
            return response.StatusCode;
        }
    }
}