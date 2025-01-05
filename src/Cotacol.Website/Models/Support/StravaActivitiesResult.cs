using Cotacol.Website.Models.CotacolApi;

namespace Cotacol.Website.Models.Support;

public class StravaActivitiesResult
{
    public int StravaActivityCount { get; set; }
    public int MissingActivityCount { get; set; }
    public string Error { get; set; }
    // public List<ActivityDetail> ExistingActivities { get; set; }
    // public List<ActivityDetail> NewActivities { get; set; }
}