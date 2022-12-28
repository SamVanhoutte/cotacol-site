namespace Cotacol.Website.Models.CotacolApi
{
    public class SyncRequest
    {
        public bool FullSync { get; set; } 
        public int MaxActivityCount { get; set; } 
        public bool ForceSync { get; set; } 
    }
}