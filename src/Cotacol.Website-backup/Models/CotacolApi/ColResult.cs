namespace Cotacol.Website.Models.CotacolApi
{
    public record ColResult    {
        public string CotacolId { get; set; } 
        public string CotacolName { get; set; } 
        public int CotacolPoints { get; set; } 
        public List<Achievement> Achievements { get; set; }
    }
    
}