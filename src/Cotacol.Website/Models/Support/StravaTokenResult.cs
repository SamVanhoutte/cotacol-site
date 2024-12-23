namespace Cotacol.Website.Models.Support;

public class StravaTokenResult
{
    public bool? ValidTokens { get; set; }
    public bool CanUpdateDescriptions { get; set; }
    public bool CanReadActivities { get; set; }
    public string Error { get; set; }
}