using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CotacolApp.Interfaces;
using CotacolApp.Models;
using CotacolApp.Models.CotacolApi;
using CotacolApp.Settings;
using Flurl.Http;
using GuardNet;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CotacolApp.Services
{
    public class CotacolApiClient : ICotacolClient
    {
        private CotacolApiSettings _settings;
        private ILogger<CotacolApiClient> _logger;

        public CotacolApiClient(IOptions<CotacolApiSettings> apiSettings, ILogger<CotacolApiClient> logger)
        {
            Guard.NotNull(apiSettings?.Value, nameof(apiSettings));
            _logger = logger;
            _settings = apiSettings.Value;
        }

        // private async Task<List<ClimbData>> GetColsFromResource()
        // {
        //     var assembly = Assembly.GetAssembly(typeof(CotacolApiClient));
        //     var resourceName = "CotacolApp.StaticData.cotacoldata.json";
        //     await using var stream = assembly.GetManifestResourceStream(resourceName);
        //     using var reader = new StreamReader(stream);
        //     var result = await reader.ReadToEndAsync();
        //     var newClimbs = JsonConvert.DeserializeObject<List<ClimbData>>(result);
        //     return newClimbs;
        // }

        public async Task<List<ClimbData>> GetClimbDataAsync()
        {
            var fullData = await $"{_settings.ApiUrl}/cotacoldata"
                .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
                //.GetStringAsync();
                //return new List<ClimbData>();
                .GetJsonAsync<List<ClimbData>>();

            return fullData;
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
            var stats = await $"{_settings.ApiUrl}/home/stats"
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
                : $"{_settings.ApiUrl}/climbs/{cotacolId}/{userId}";
            var segmentData = await url
                .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
                .GetJsonAsync<ClimbUserDetail>();

            return segmentData;
        }

        public async Task<StravaSegmentResponse> FetchStravaSegmentAsync(string stravaSegmentId)
        {
            if (stravaSegmentId.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
            {
                stravaSegmentId = stravaSegmentId.Split('/').Last();
            }

            var segmentResponse = await $"{_settings.ApiUrl}/climbs/segment/{stravaSegmentId}"
                .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
                .GetJsonAsync<StravaSegmentResponse>();

            return segmentResponse;
        }

        public async Task<int> UpdateSegmentAsync(string cotacolId, UpdateSegmentRequest update)
        {
            var response = await $"{_settings.ApiUrl}/climbs/{cotacolId}"
                .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
                .PostJsonAsync(update);
            return response.StatusCode;
        }

        public async Task<List<UserRecord>> GetUsersAsync()
        {
            return await $"{_settings.ApiUrl}/users"
                .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
                .GetJsonAsync<List<UserRecord>>();
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
    }
}