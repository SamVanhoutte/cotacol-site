using System;
using Newtonsoft.Json;

namespace CotacolApp.Models.CotacolApi
{
    public record UserProfile
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public bool TestUser { get; set; }
        [JsonProperty("settings")] public UserSettings UserSettings { get; set; }
        public DateTime LastSyncUtc { get; set; }
    }
}