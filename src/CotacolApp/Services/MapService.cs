using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CotacolApp.Models;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;

namespace CotacolApp.Services
{
    public class MapService
    {
        private List<Marker> _markers = new();
        private List<Polyline> _lines = new();
        private MarkerClustering _markerClustering;

        public async Task<MapOptions> GetLayoutAsync(bool miniView = false)
        {
            return new()
            {
                Zoom = 9,
                Center = new LatLngLiteral(4.35, 50.85),
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

        public async Task ClusterMarkersAsync(GoogleMap map1)
        {
            _markerClustering = await MarkerClustering.CreateAsync(map1.JsRuntime, map1.InteropObject, _markers);
            await _markerClustering.FitMapToMarkers(0);
        }

        public async Task ShowClimbAsync(GoogleMap map1, UserClimb climb, bool plotTrack = true, bool singleClimb = false)
        {
            if (!string.IsNullOrEmpty(climb.Polyline))
            {
                var polyline = Columbae.Polyline.ParsePolyline(climb.Polyline);
                await AddMarkerAsync(map1, climb, polyline, singleClimb);
                if(plotTrack) await DrawPolylineAsync(map1, polyline);
                if (singleClimb) await ZoomToClimbAsync(map1, polyline);
            }
        }

        private async Task ZoomToClimbAsync(GoogleMap map1, Columbae.Polyline polyline)
        {
            var box = polyline.BoundingBox;
            var bounds = new LatLngBoundsLiteral(
                new LatLngLiteral(box.Vertices.First().Longitude, box.Vertices.First().Latitude),
                new LatLngLiteral(box.Vertices[2].Longitude, box.Vertices[2].Latitude));
            await map1.InteropObject.FitBounds(bounds);
        }

        private async Task AddMarkerAsync(GoogleMap map1, UserClimb climb, Columbae.Polyline polyline,
            bool singleClimb = false)
        {
            var start = polyline.Vertices.First();
            var marker = await Marker.CreateAsync(map1.JsRuntime, new MarkerOptions()
            {
                Position = new LatLngLiteral(start.Longitude, start.Latitude),
                Map = map1.InteropObject,
                Icon = new Icon {Url = "images/climb-icon-map-pink.png"}
            });
            if (singleClimb)
            {
                // await marker.SetLabelText(climb.Name);
                await marker.SetAnimation(Animation.Drop);
            }

            var infoWindowContent = $@"<table class='table table-striped table-dark'><tbody>
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
            var infoWindow = await InfoWindow.CreateAsync(map1.JsRuntime, new InfoWindowOptions()
            {
                Position = new LatLngLiteral(start.Longitude, start.Latitude)
            });
            await marker.AddListener("click", async () =>
            {
                await infoWindow.SetContent(infoWindowContent);
                await infoWindow.SetPosition(new LatLngLiteral(start.Longitude, start.Latitude));
                await infoWindow.Open(map1.InteropObject);
            });
            _markers.Add(marker);
        }


        private async Task DrawPolylineAsync(GoogleMap map1, Columbae.Polyline polyline)
        {
            var path = polyline.Vertices.Select(v => new LatLngLiteral(v.Longitude, v.Latitude));
            var line = await Polyline.CreateAsync(map1.JsRuntime, new PolylineOptions()
            {
                StrokeColor = "#FD7D7A",
                Clickable = true,
                Draggable = false,
                Editable = false,
                Map = map1.InteropObject, Path = path
            });
            _lines.Add(line);
        }

        public async Task ClearClimbsAsync(GoogleMap map1)
        {
            var tasks = new List<Task> { };
            tasks.AddRange(
                _markers.Select(marker => marker.SetMap(null)));
            await Task.WhenAll(tasks);
            
            tasks = new List<Task> { };
            tasks.AddRange(
                _lines.Select(line => line.SetMap(null)));
            await Task.WhenAll(tasks);
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