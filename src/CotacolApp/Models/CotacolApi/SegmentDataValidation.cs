using System;

namespace CotacolApp.Models.CotacolApi
{
    public class SegmentDataValidation
    {
        public string CotacolId { get; set; }
        public string SegmentId { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public string SegmentName { get; set; }
        public string SegmentConfidence { get; set; }
        public double CotacolLength { get; set; }
        public double SegmentLength { get; set; }
        public DateTime SegmentUpdated { get; set; }
        public DateTime ClimbUpdated { get; set; }
        public double LengthDifference => SegmentLength==0 ? 0: Math.Abs(SegmentLength - CotacolLength);
    }
}