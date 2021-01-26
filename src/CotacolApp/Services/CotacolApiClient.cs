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
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

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
        
        private async Task<List<ClimbData>> GetColsFromResource()
        {
            var assembly = Assembly.GetAssembly(typeof(CotacolApiClient));
            var resourceName = "CotacolApp.StaticData.cotacoldata.json";
            await using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);
            var result = await reader.ReadToEndAsync();
            var newClimbs = JsonConvert.DeserializeObject<List<ClimbData>>(result);
            return newClimbs;
        }
        
        public async Task<List<ClimbData>> GetClimbDataAsync()
        {
            var fullData = await GetColsFromResource();
            
            var segmentData = await $"{_settings.ApiUrl}/climbs"
                .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
                .GetJsonAsync<List<Climb>>();

            foreach (var climbData in fullData)
            {
                climbData.StravaSegment = segmentData.FirstOrDefault(sd =>
                    sd.Id.Equals(climbData.Id, StringComparison.InvariantCultureIgnoreCase))?.Url;
            }

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
            // TODO : valdiation of required props
            var response = await $"{_settings.ApiUrl}/user/{userSettings.UserId}"
                .WithHeader(_settings.SharedKeyHeaderName, _settings.SharedKeyValue)
                .PostJsonAsync(userSettings);

            return response.ResponseMessage.IsSuccessStatusCode;
        }
    }
}