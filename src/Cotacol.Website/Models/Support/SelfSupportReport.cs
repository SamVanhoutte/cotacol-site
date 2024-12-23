using System.Text.Json;

namespace Cotacol.Website.Models.Support;

public class SelfsupportReport
{
    public SelfsupportReport()
    {
        ReportTimestampUtc = DateTime.UtcNow;
        UserResult = new UserResult();
        StravaTokenResult = new StravaTokenResult();
        MissingActivitiesResult = new MissingActivitiesResult();
        StravaActivitiesResult = new StravaActivitiesResult();
        SyncActivitiesResult = new SyncActivitiesResult();
    }

    public DateTime ReportTimestampUtc { get; set; }
    public UserResult UserResult { get; set; }
    public StravaTokenResult StravaTokenResult { get; set; }
    public MissingActivitiesResult MissingActivitiesResult { get; set; }
    public StravaActivitiesResult StravaActivitiesResult { get; set; }
    public SyncActivitiesResult SyncActivitiesResult { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions{WriteIndented = true});
    }
}