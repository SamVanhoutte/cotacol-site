namespace CotacolApp.Services.Maps
{
    public struct MapLayout
    {
        public bool ShowPolylines { get; set; }
        public bool ShowMarkers { get; set; }
        public bool ShowStartFinish { get; set; }
        public bool RedrawNeeded { get; set; }
        public bool CenterMap { get; set; }
        public bool SingleClimb { get; set; }
    }
}