using Cotacol.Website.Interfaces;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Extension;
using Microsoft.JSInterop;
using Columbae;
using Columbae.World;
using Cotacol.Website.Models;
using Cotacol.Website.Shared;
using MudBlazor;
using MouseEvent = GoogleMapsComponents.Maps.MouseEvent;
using Route = Columbae.World.Route;
    
namespace Cotacol.Website.Services.Maps
{
    public class MapListService : MapService, IMapService
    {
        private MarkerList _markerList;
        private PolylineList _lineList;
        private ILogger<MapListService> _logger;

        public Route RouteToPlot
        {
            get;
            set;
        }
        public MapListService(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger<MapListService>();
        }

        protected override Task<List<Marker>> GetMarkersAsync()
        {
            return Task.FromResult(_markerList.Markers.Values.ToList());
        }


        public async Task PlotRoute(Map map1, IJSRuntime jsRuntime, Route route)
        {
            var color = "green";
            RouteToPlot = route;
            var path = route.Vertices.Select(v => new LatLngLiteral(v.Latitude, v.Longitude));
            var options = new PolylineOptions()
            {
                StrokeColor = color,
                Clickable = false,
                Draggable = false,
                Editable = false, StrokeOpacity = 0f,
                Map = map1, Path = path
            };
            options.Icons = new[]
            {
                new IconSequence
                {
                    Icon = new Symbol
                    {
                        Path = SymbolPath.CIRCLE, Scale = 2f, StrokeColor = color, StrokeOpacity = 1f, FillOpacity = 1f,
                        FillColor = color
                    },
                    Offset = "0%", Repeat = "10px"
                },
                new IconSequence
                {
                    Icon = new Symbol {Path = SymbolPath.FORWARD_OPEN_ARROW, Scale = 2f, StrokeColor = color},
                    Offset = "25%"
                },
                new IconSequence
                {
                    Icon = new Symbol {Path = SymbolPath.FORWARD_OPEN_ARROW, Scale = 2f, StrokeColor = color},
                    Offset = "75%"
                },
            };
            if (_lineList == null)
            {
                _lineList = await PolylineList.CreateAsync(jsRuntime, new Dictionary<string, PolylineOptions>());
            }
            await _lineList.AddMultipleAsync(new Dictionary<string, PolylineOptions> {{"route", options}});
        }

        public async Task ClearClimbsAsync()
        {
        }

        private async Task ClearClusterAsync()
        {
            if (MarkerCluster != null)
            {
                await MarkerCluster.ClearMarkers();
                await MarkerCluster.SetMap(null);
            }
        }

        public async Task ShowClimbAsync(Map map1, IJSRuntime jsRuntime, UserClimb climb,
            MapLayout mapLayout)
        {
            await ShowClimbsAsync(map1, jsRuntime, new List<UserClimb> {climb}, mapLayout);
        }

        public async Task ShowClimbsAsync(Map map1, IJSRuntime jsRuntime, List<UserClimb> climbs,
            MapLayout mapLayout, Action<MouseEvent, string> handleClick = null)
        {
            _logger.LogInformation($"Showing multiple climbs {climbs.Count()}");

            var climbsToShow = climbs.Where(c => !string.IsNullOrEmpty(c.Polyline));

            if (mapLayout.ShowPolylines == false) await RemoveLinesAsync();

            if (mapLayout.ShowMarkers == false)
            {
                await RemoveMarkersAsync();
            }

            if (mapLayout.ShowMarkers)
            {
                await AddMarkersAsync(map1, jsRuntime, climbsToShow);
                if (handleClick != null)
                {
                    await _markerList.AddListeners(_markerList.Markers.Keys, "click", handleClick);
                }
            }

            if (mapLayout.ShowPolylines)
            {
                await AddTracksAsync(map1, jsRuntime, mapLayout.ShowStartFinish, climbsToShow);
                if (handleClick != null)
                {
                    await _lineList.AddListeners(_markerList.Markers.Keys, "click", handleClick);
                }
            }

            if (climbs.Count.Equals(1))
            {
                var l = Columbae.Polyline.ParsePolyline(climbs.First().Polyline);
                await ZoomToClimbAsync(map1, l);
            }

            if (RouteToPlot != null)
            {
                await PlotRoute(map1, jsRuntime, RouteToPlot);
            }
        }

        private async Task AddTracksAsync(Map map1, IJSRuntime jsRuntime, bool showArrow, IEnumerable<UserClimb> climbs)
        {
            if (_lineList == null)
            {
                _lineList = await PolylineList.CreateAsync(jsRuntime, new Dictionary<string, PolylineOptions>());
            }

            await _lineList.SetMultipleAsync(
                climbs.ToDictionary(
                    s => s.Id, c => GetClimbLine(c, showArrow, map1)));
        }

        private async Task AddMarkersAsync(Map map1, IJSRuntime jsRuntime, IEnumerable<UserClimb> climbs)
        {
            if (_markerList == null)
            {
                _markerList = await MarkerList.CreateAsync(
                    jsRuntime, new Dictionary<string, MarkerOptions>());
            }

            await _markerList.SetMultipleAsync(
                climbs.ToDictionary(
                    s => s.Id, c => GetClimbMarker(c, map1)));
            await ClusterMarkersAsync(map1, jsRuntime);
        }


        private PolylineOptions GetClimbLine(UserClimb climb, bool showArrow, Map map1)
        {
            var polyline = Columbae.Polyline.ParsePolyline(climb.Polyline);
            var path = polyline.Vertices.Select(v => new LatLngLiteral(v.Latitude, v.Longitude));
            var options = new PolylineOptions()
            {
                StrokeColor = climb.Done ? MainLayout.CotacolTheme.Palette.Success.Value : MainLayout.CotacolTheme.Palette.Secondary.Value,
                Clickable = true,
                Draggable = false,
                Editable = false,
                Map = map1, Path = path
            };
            if (showArrow)
            {
                options.Icons = new[]
                {
                    new IconSequence
                    {
                        Icon = new Symbol {Path = SymbolPath.CIRCLE, Scale = 3f, StrokeColor = "green"},
                        Offset = "0%"
                    },
                    new IconSequence
                    {
                        Icon = new Symbol {Path = SymbolPath.CIRCLE, Scale = 3f, StrokeColor = "red"},
                        Offset = "100%"
                    },
                    // new IconSequence
                    // {
                    //     Icon = new Symbol {Path = SymbolPath.FORWARD_OPEN_ARROW, Scale = 3f},
                    //     Offset = "50%"
                    // },
                };
            }

            return options;
        }


        private async Task RemoveMarkersAsync()
        {
            if (_markerList != null)
            {
                var tasks = new List<Task>();
                tasks.AddRange(_markerList.Markers.Select(marker => marker.Value.SetMap(null)));
                await Task.WhenAll(tasks);
                await _markerList.RemoveAllAsync();
            }

            await ClearClusterAsync();
        }

        private async Task RemoveLinesAsync()
        {
            if (_lineList != null)
            {
                var tasks = new List<Task>();
                tasks.AddRange(_lineList.Polylines.Select(line => line.Value.SetMap(null)));
                await Task.WhenAll(tasks);
                await _lineList.RemoveAllAsync();
            }
        }
    }
}