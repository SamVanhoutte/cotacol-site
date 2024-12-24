using System.Text.Json;

namespace Cotacol.Website.Models.Support;

public class ColSearchReport
{
    public ColSearchReport()
    {
        ReportTimestampUtc = DateTime.UtcNow;
        UserResult = new UserResult();
        StravaTokenResult = new StravaTokenResult();
        MissingData = new MissingData();
        MissingActivitiesResult = new MissingActivitiesResult();
        ActivitiesResult = new CotacolActivitiesResult();
        SyncedActivityResult = new SyncActivitiesResult();
        SegmentValidationResult = new SegmentValidationResult();
    }

    public DateTime ReportTimestampUtc { get; set; }
    public UserResult UserResult { get; set; }
    public StravaTokenResult StravaTokenResult { get; set; }
    public MissingActivitiesResult MissingActivitiesResult { get; set; }
    public CotacolActivitiesResult ActivitiesResult { get; set; }
    public MissingData MissingData { get; set; }
    public SyncActivitiesResult SyncedActivityResult { get; set; }
    public SegmentValidationResult SegmentValidationResult { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions{WriteIndented = true});
    }
}