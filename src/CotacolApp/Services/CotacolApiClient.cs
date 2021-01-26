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

        public Task<SiteStats> GetStatsAsync()
        {
            throw new System.NotImplementedException();
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