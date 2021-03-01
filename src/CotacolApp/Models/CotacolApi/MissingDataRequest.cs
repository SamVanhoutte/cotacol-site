namespace CotacolApp.Models.CotacolApi
{
    public class MissingDataRequest
    {
        public string UserId { get; set; }
        public string Remark { get; set; }
        public string CotacolId { get; set; }
        public string ActivityId { get; set; }
    }
}