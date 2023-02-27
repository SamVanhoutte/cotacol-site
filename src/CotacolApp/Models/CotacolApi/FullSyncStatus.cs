using System;

namespace CotacolApp.Models.CotacolApi;

public class FullSyncStatus
{
    public bool Succeeded { get; set; }
    public DateTime SyncTime { get; set; }
    public string ErrorDescription { get; set; }
    public string Trigger { get; set; }
    public string ErrorCode { get; set; }
}