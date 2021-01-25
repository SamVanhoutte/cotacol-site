using System.Collections.Generic;
using System.Threading.Tasks;
using CotacolApp.Interfaces;
using CotacolApp.Models.CotacolApi;
using CotacolApp.Settings;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;

namespace CotacolApp.Services
{
    public class CotacolApiClient : ICotacolClient
    {
        private CotacolApiSettings _settings;

        public CotacolApiClient()
        {
            // TODO : options
            _settings = new CotacolApiSettings {BaseUrl =  "https://cotacol-hunting.azure-api.net/test/api/v1", SecretKeyName = "x-api-subscription-key"};
        }
        public async Task<List<Climb>> GetClimbDataAsync()
        {
            var climbs = await $"{_settings.BaseUrl}/climbs"
                .WithHeader(_settings.SecretKeyName, "8963ff1421164023ac3d789567a58896")
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
            var response = await $"{_settings.BaseUrl}/user/{userSettings.UserId}"
                .WithHeader(_settings.SecretKeyName, "8963ff1421164023ac3d789567a58896")
                .PostJsonAsync(userSettings);

            return response.ResponseMessage.IsSuccessStatusCode;
        }
    }
}