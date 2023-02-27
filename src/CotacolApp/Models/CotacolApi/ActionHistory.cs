using System;

namespace CotacolApp.Models.CotacolApi;

public class ActionHistory
{
    public DateTime? IOSLogin { get; set; }
    public DateTime? AndroidLogin { get; set; }
    public DateTime? WebLogin { get; set; }
    public DateTime? FullSync { get; set; }
    public DateTime? ActivityWebhookSync { get; set; }
}