using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CotacolApp.Models;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Extension;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace CotacolApp.Services.Maps
{
    public class MapListService : MapService, IMapService
    {
        private MarkerList _markerList;
        private PolylineList _lineList;
        private ILogger<MapListService> _logger;

        public MapListService(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger<MapListService>();
        }

        protected override Task<List<Marker>> GetMarkersAsync()
        {
            return Task.FromResult(_markerList.Markers.Values.ToList());
        }

        public async Task ClearClimbsAsync()
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
            MapLayout mapLayout)
        {
            _logger.LogInformation($"Showing multiple climbs {climbs.Count()}");

            var climbsToShow = climbs.Where(c => !string.IsNullOrEmpty(c.Polyline));
            if (mapLayout.ShowPolylines == false)
            {
                // Removing the polylines
                await RemoveLinesAsync();
            }

            if (mapLayout.ShowMarkers == false)
            {
                // Removing the markers
                await RemoveMarkersAsync();
            }

            if (mapLayout.ShowMarkers)
            {
                // Showing the actual lines
                await AddMarkersAsync(map1, jsRuntime, climbsToShow);
                //await ClusterMarkersAsync(map1, jsRuntime);
            }

            if (mapLayout.ShowPolylines)
            {
                // Showing the lines
                await AddTracksAsync(map1, jsRuntime, mapLayout.ShowArrows, climbsToShow);
            }

            if (climbs.Count.Equals(1))
            {
                var l = Columbae.Polyline.ParsePolyline(climbs.First().Polyline);
                await ZoomToClimbAsync(map1, l);
            }
        }

        private async Task AddTracksAsync(Map map1, IJSRuntime jsRuntime, bool showArrow, IEnumerable<UserClimb> climbs)
        {
            if (climbs.Any())
                _logger.LogInformation("****ADDING LINES NOW");

            if (_lineList == null)
            {
                _lineList = await PolylineList.CreateAsync(
                    jsRuntime, climbs
                        .ToDictionary(s => s.Id, c => GetClimbLine(c, showArrow, map1))
                );
            }
            else
            {
                await _lineList.AddMultipleAsync(
                    climbs.ToDictionary(
                        s => s.Id, c => GetClimbLine(c, showArrow, map1)));

                //await RemoveLinesAsync(climbs);
            }
        }

        private PolylineOptions GetClimbLine(UserClimb climb, bool showArrow, Map map1)
        {
            var polyline = Columbae.Polyline.ParsePolyline(climb.Polyline);
            var path = polyline.Vertices.Select(v => new LatLngLiteral(v.Longitude, v.Latitude));
            var options = new PolylineOptions()
            {
                StrokeColor = "#FD7D7A",
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
                    // new IconSequence {Icon = new Symbol {Path = SymbolPath.FORWARD_OPEN_ARROW}, Offset = "80%"},
                    // new IconSequence
                    // {
                    //     Icon = new Symbol {Path = SymbolPath.FORWARD_OPEN_ARROW, Scale = 3f},
                    //     Offset = "50%"
                    // },
                    // new IconSequence {Icon = new Symbol {Path = SymbolPath.FORWARD_OPEN_ARROW}, Offset = "40%"},
                    // new IconSequence {Icon = new Symbol {Path = SymbolPath.FORWARD_OPEN_ARROW}, Offset = "20%"},
                };
            }

            return options;
        }

        private async Task AddMarkersAsync(Map map1, IJSRuntime jsRuntime, IEnumerable<UserClimb> climbs)
        {
            _logger.LogInformation("****ADDING MARKERS NOW");
            if (_markerList == null)
            {
                _markerList = await MarkerList.CreateAsync(
                    jsRuntime, climbs
                        .ToDictionary(s => s.Id, c => GetClimbMarker(c, map1))
                );
            }
            else
            {
                await _markerList.SetMultipleAsync(
                    climbs.ToDictionary(
                        s => s.Id, c => GetClimbMarker(c, map1)));

                //await RemoveMarkersAsync(climbs);
            }
        }

        private async Task RemoveMarkersAsync()
        {
            if (_markerList != null)
            {
                _logger.LogInformation($"****** CLEARING MARKERS *********");
                var markerIds = new List<string> { };
                foreach (var mrk in _markerList.Markers)
                {
                    markerIds.Add(mrk.Key);
                    await mrk.Value.SetMap(null);
                }

                await _markerList.RemoveMultipleAsync(markerIds);
            }
        }

        private async Task RemoveLinesAsync()
        {
            if (_lineList != null)
            {
                _logger.LogInformation($"****** CLEARING LINES *********");

                var lineIds = new List<string> { };
                foreach (var line in _lineList.Polylines)
                {
                    lineIds.Add(line.Key);
                    await line.Value.SetMap(null);
                }

                await _lineList.RemoveMultipleAsync(lineIds);
            }
        }

        private async Task RemoveUnneededMarkersAsync(IEnumerable<UserClimb> climbs)
        {
            if (climbs.Count() < 1000)
            {
                await _markerList.RemoveMultipleAsync();
                var keysToRemove = Enumerable.Range(1, 1000)
                    .Where(colId => climbs.Select(c => c.Id).Contains(colId.ToString()) == false)
                    .Select(colId => colId.ToString())
                    .ToList();
                foreach (var markerId in keysToRemove.Where(
                    markerId => _markerList.Markers.ContainsKey(markerId)))
                {
                    await _markerList.Markers[markerId].SetMap(null);
                }

                await _markerList.RemoveMultipleAsync(keysToRemove);
            }
        }


        private async Task RemoveUnneededLinesAsync(IEnumerable<UserClimb> climbs)
        {
            if (climbs.Count() < 1000)
            {
                // Remove keys from marker list that should not be shown
                var keysToRemove = Enumerable.Range(1, 1000)
                    .Where(colId => climbs.Select(c => c.Id).Contains(colId.ToString()) == false)
                    .Select(colId => colId.ToString())
                    .ToList();
                foreach (var lineId in keysToRemove.Where(
                    markerId => _lineList.Polylines.ContainsKey(markerId)))
                {
                    await _lineList.Polylines[lineId].SetMap(null);
                }

                await _lineList.RemoveMultipleAsync(keysToRemove);
            }
        }
    }
}