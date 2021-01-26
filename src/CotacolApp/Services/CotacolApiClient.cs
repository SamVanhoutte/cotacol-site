using System.Collections.Generic;
using System.Threading.Tasks;
using CotacolApp.Interfaces;
using CotacolApp.Models.CotacolApi;
using CotacolApp.Settings;
using Flurl.Http;
using GuardNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CotacolApp.Services
{
    public class CotacolApiClient : ICotacolClient
    {
        private CotacolApiSettings _settings;

        public CotacolApiClient(IOptions<CotacolApiSettings> apiSettings)
        {
            Guard.NotNull(apiSettings?.Value, nameof(apiSettings));
            _settings = apiSettings.Value;
        }
        public async Task<List<Climb>> GetClimbDataAsync()
        {
            var climbs = await $"{_settings.ApiUrl}/climbs"
                .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
                .GetJsonAsync<List<Climb>>();

            return climbs;
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
            // TODO : valdiation of required props
            var response = await $"{_settings.ApiUrl}/user/{userSettings.UserId}"
                .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
                .PostJsonAsync(userSettings);

            return response.ResponseMessage.IsSuccessStatusCode;
        }
    }
}