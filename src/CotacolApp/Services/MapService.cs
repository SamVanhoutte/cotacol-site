using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Columbae;
using CotacolApp.Models;
using CotacolApp.Models.CotacolApi;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.JSInterop;
using Serilog.Core;
using Polyline = GoogleMapsComponents.Maps.Polyline;

namespace CotacolApp.Services
{
    public class MapService
    {
        private List<Marker> _markers = new();
        private List<Polyline> _lines = new();
        private MarkerClustering _markerClustering;

        public async Task<MapOptions> GetLayoutAsync(bool miniView = false, int zoom = 9)
        {
            return new()
            {
                Zoom = zoom,
                Center = new LatLngLiteral(4.52, 50.28),
                MapTypeId = MapTypeId.Roadmap,
                ZoomControl = !miniView,
                Scrollwheel = true,
                Draggable = true,
                MapTypeControl = !miniView,
                Styles = await GetMapStylesAsync()
            };
        }

        private static async Task<MapTypeStyle[]> GetMapStylesAsync()
        {
            var style = new GoogleMapStyleBuilder();
            var assembly = Assembly.GetAssembly(typeof(MapService));
            var resourceName = "CotacolApp.StaticData.cotacolmapstyle.json";
            await using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    var result = await reader.ReadToEndAsync();
                    style.AddStyle(result);
                }
            }

            return style.Build();
        }

        public async Task ClusterMarkersAsync(Map map1, IJSRuntime jsRuntime, bool fitMarkers = false)
        {
            _markerClustering = await MarkerClustering.CreateAsync(jsRuntime, map1, _markers);
            if (fitMarkers) await _markerClustering.FitMapToMarkers(0);
        }

        public async Task ShowClimbAsync(Map map1, IJSRuntime jsRuntime, UserClimb climb,
            bool plotTrack = true, bool singleClimb = false, bool showArrows = false)
        {
            if (!string.IsNullOrEmpty(climb.Polyline))
            {
                var polyline = Columbae.Polyline.ParsePolyline(climb.Polyline);

                if (!plotTrack) await AddMarkerAsync(map1, jsRuntime, climb, polyline, singleClimb);
                if (plotTrack) await DrawPolylineAsync(map1, jsRuntime, climb, polyline, true);
                if (singleClimb) await ZoomToClimbAsync(map1, polyline);
            }
        }

        private async Task ZoomToClimbAsync(Map map1, Columbae.Polyline polyline)
        {
            var box = polyline.BoundingBox;
            var bounds = new LatLngBoundsLiteral(
                new LatLngLiteral(box.Vertices.First().Longitude, box.Vertices.First().Latitude),
                new LatLngLiteral(box.Vertices[2].Longitude, box.Vertices[2].Latitude));
            await map1.FitBounds(bounds);
        }

        private async Task AddMarkerAsync(Map map1, IJSRuntime jsRuntime, UserClimb climb, Columbae.Polyline polyline,
            bool singleClimb = false)
        {
            var start = polyline.Vertices.First();
            var marker = await Marker.CreateAsync(jsRuntime, new MarkerOptions()
            {
                Position = new LatLngLiteral(start.Longitude, start.Latitude),
                Map = map1,
                Icon = new Icon {Url = "images/climb-icon-map-pink.png"}
            });
            if (singleClimb)
            {
                // await marker.SetLabelText(climb.Name);
                await marker.SetAnimation(Animation.Drop);
            }

            var infoWindow = await CreateInfoWindowAsync(climb, jsRuntime, start);
            await marker.AddListener("click", async () =>
            {
                await infoWindow.Open(map1);
            });
            _markers.Add(marker);
        }

        private async Task<InfoWindow> CreateInfoWindowAsync(ClimbData climb,IJSRuntime jsRuntime, Polypoint position)
        {
            var infoWindowContent = $@"<table class='table table-striped table-dark'><tbody>
                        <tr>
                        <td colspan=2>{climb.Name}</td>
                        </tr>
                        <tr>
                        <td>Points</td>
                        <td>{climb.CotacolPoints}</td>
                        </tr>
                        <tr>
                        <td>Length</td>
                        <td>{climb.Distance}</td>
                        </tr>
                        <tr>
                        <td>Elevation</td>
                        <td>{climb.ElevationDiff}</td>
                        </tr>
                        </tbody>
                        </table>";
            var infoWindow = await InfoWindow.CreateAsync(jsRuntime, new InfoWindowOptions()
            {
                Position = new LatLngLiteral(position.Longitude, position.Latitude)
            });
            await infoWindow.SetContent(infoWindowContent);
            await infoWindow.SetPosition(new LatLngLiteral(position.Longitude, position.Latitude));
            return infoWindow;
        }
        private async Task DrawPolylineAsync(Map map1, IJSRuntime jsRuntime, ClimbData climb, Columbae.Polyline polyline,
            bool showArrow = false)
        {
            var path = polyline.Vertices.Select(v => new LatLngLiteral(v.Longitude, v.Latitude));
            var line = await Polyline.CreateAsync(jsRuntime, new PolylineOptions()
            {
                StrokeColor = "#FD7D7A",
                Clickable = true,
                Draggable = false,
                Editable = false,
                Map = map1, Path = path
            });
            if (showArrow)
            {
                await line.SetOptions(new PolylineOptions
                {
                    Icons = new[]
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
                    }
                });
                var infoWindow = await CreateInfoWindowAsync(climb, jsRuntime, polyline.Vertices.First());
                await line.AddListener("click", async () =>
                {
                    await infoWindow.Open(map1);
                });
            }

            _lines.Add(line);
        }

        public async Task ClearClimbsAsync()
        {
            if (_markers != null)
            {
                var tasks = new List<Task> { };
                tasks.AddRange(
                    _markers.Select(marker => marker.SetMap(null)));
                await Task.WhenAll(tasks);
            }

            if (_lines != null)
            {
                var tasks = new List<Task> { };
                tasks.AddRange(
                    _lines.Select(line => line.SetMap(null)));
                await Task.WhenAll(tasks);
            }

            if (_markerClustering != null)
            {
                await _markerClustering.ClearMarkers();
                await _markerClustering.SetMap(null);
            }

            _markers = new List<Marker>();
            _lines = new List<Polyline>();
        }
    }
}