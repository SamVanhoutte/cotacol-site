using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CotacolApp.Interfaces;
using CotacolApp.Models;
using CotacolApp.Models.CotacolApi;
using CotacolApp.Settings;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using SQLitePCL;

namespace CotacolApp.Services
{
    public class CotacolApiUserClient : ICotacolUserClient
    {
        private CotacolApiSettings _settings;
        private IHttpContextAccessor _context;
        private ICotacolClient _cotacolDataClient;

        public CotacolApiUserClient(IHttpContextAccessor contextAccessor, ICotacolClient cotacolDataClient)
        {
            // TODO : options
            _settings = new CotacolApiSettings
            {
                BaseUrl = "https://cotacol-hunting.azure-api.net/test/api/v1", SecretKeyName = "x-api-subscription-key"
            };
            _context = contextAccessor;
            _cotacolDataClient = cotacolDataClient;
        }

        private string GetUserId()
        {
            var authenticatedUser = _context.HttpContext.User.GetUserId();
            return authenticatedUser;
        }

        public async Task<List<UserClimb>> GetClimbDataAsync(string userId)
        {
            var colsDone = await GetColsAsync(userId);
            var allClimbs = (await _cotacolDataClient.GetClimbDataAsync()).Select(climb => climb.ToUserClimb());

            foreach (var climb in allClimbs)
            {
                if (colsDone.Any(c => c.CotacolId.Equals(climb.Id)))
                {
                    climb.Done = true;
                }
            }
            // foreach (var userColAchievement in colsDone)
            // {
            //     var doneClimb = allClimbs.First(climb => climb.Id.Equals(userColAchievement.CotacolId));
            //     doneClimb.Done = true;
            //     doneClimb.FirstAchievement = userColAchievement.AchievementDate;
            // }

            return allClimbs.ToList();
        }

        public async Task<List<UserColAchievement>> GetColsAsync(string userId)
        {
            var cols = await $"{_settings.BaseUrl}/user/{GetUserId()}/cols"
                .WithHeader(_settings.SecretKeyName, "8963ff1421164023ac3d789567a58896")
                .GetJsonAsync<List<UserColAchievement>>();

            return cols;
        }

        public Task<List<CotacolActivity>> GetActivitiesAsync(string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<UserAchievements> GetAchievementsAsync(string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<UserProfile> GetProfileAsync(string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<SyncStatus> GetSyncStatus(string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}