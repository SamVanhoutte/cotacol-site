using Cotacol.Website.Models.CotacolApi;
using Cotacol.Website.Models.CotacolApi.Strava;

namespace Cotacol.Website.Models.Support;

public class MissingActivitiesResult
{
    public int SynchedActivities { get; set; }
    public string Error { get; set; }
    public int MissingActivities { get; set; }
}

public class MissingData
{
    public CotacolActivity? Activity { get; set; }
    public UserClimb? Cotacol { get; set; }
    public StravaActivityDetailResponse StravaActivityDetails { get; set; }
    public ClimbDetail ColDetail { get; set; }
}