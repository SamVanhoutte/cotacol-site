using Cotacol.Website.Models.CotacolApi;

namespace Cotacol.Website.Models.Support;

public class CotacolActivitiesResult
{
    public int StravaActivityCount { get; set; }
    public string Error { get; set; }
    //public List<CotacolActivity> Activities { get; set; }
    public List<UserClimb> UserCols { get; set; }
}