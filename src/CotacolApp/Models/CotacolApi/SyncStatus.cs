namespace CotacolApp.Models.CotacolApi
{
    public record SyncStatus    {
        public string UserId { get; set; } 
        public bool SyncActive { get; set; } 
        public int Processed { get; set; } 
        public int Total { get; set; } 
        public string Status { get; set; } 
    }
}