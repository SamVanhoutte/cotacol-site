namespace Cotacol.Website.Models.Support;

public class MissingActivitiesResult
{
    public int SynchedActivities { get; set; }
    public string Error { get; set; }
    public int MissingActivities { get; set; }
}