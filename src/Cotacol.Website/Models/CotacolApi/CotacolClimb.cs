namespace Cotacol.Website.Models.CotacolApi
{
    public class CotacolClimb
    {
        public string CotacolId { get; set; }
        public string CotacolName { get; set; }
        public int UniqueUsers { get; set; }
        public int TotalAttempts { get; set; }
        public List<LocalLegend> LocalLegends { get; set; }

        public int MaxAttempts
        {
            get
            {
                try
                {
                    if (LocalLegends == null || !LocalLegends.Any()) return 0;
                    return LocalLegends.Max(ll => ll.Attempts);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }
            }
        }
    }
}