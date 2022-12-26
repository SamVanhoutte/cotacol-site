namespace Cotacol.Website.Models.CotacolApi
{
    public class Cotacol
    {
        public string CotacolId { get; set; }
        public string CotacolName { get; set; }
        public int UniqueUsers { get; set; }
        public int TotalAttempts { get; set; }
        public List<LocalLegend> LocalLegends { get; set; }
    }
}