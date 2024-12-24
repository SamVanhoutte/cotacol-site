namespace Cotacol.Website.Models;

public class SegmentMatchResult
{
    public string SegmentId { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public string Name { get; set; }
    public string Reason { get; set; }
    public bool Found { get; set; }
    public bool Qualifies { get; set; }
}