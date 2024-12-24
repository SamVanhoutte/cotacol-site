namespace Cotacol.Website.Models.Support;

public class SegmentValidationResult
{
    public string Error { get; set; }
    public List<SegmentMatchResult> SegmentResults { get; set; }
    public bool QualifyingEffortFound { get; set; }
}