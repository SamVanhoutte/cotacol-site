using System;

namespace CotacolApp.Models.CotacolApi
{
    public record UserProfile    {
        public string UserId { get; set; } 
        public string UserName { get; set; } 
        public string FullName { get; set; } 
        public object EmailAddress { get; set; } 
        public UserSettings UserSettings { get; set; } 
        public DateTime LastSyncUtc { get; set; } 
    }
}