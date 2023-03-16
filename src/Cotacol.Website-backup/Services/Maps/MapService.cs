using System.Reflection;
using Columbae;
using Cotacol.Website.Models;
using Cotacol.Website.Services.Extensions;
using GoogleMapsComponents.Maps;
using Microsoft.JSInterop;

namespace Cotacol.Website.Services.Maps
{
    public abstract class MapService
    {
        protected MarkerClustering MarkerCluster;

        public async Task<MapOptions> GetLayoutAsync(bool miniView = false, double zoom = 9)
        {
            return new()
            {
                Zoom = (int)zoom,
                Center = new LatLngLiteral(4.52, 50.28),
                MapTypeId = MapTypeId.Roadmap,
                ZoomControl = !miniView,
                Scrollwheel = true,
                Draggable = true,
                MapTypeControl = !miniView,
                Styles = await GetMapStylesAsync()
            };
        }

        protected abstract Task<List<Marker>> GetMarkersAsync();

        public async Task ClusterMarkersAsync(Map map1, IJSRuntime jsRuntime, bool fitMarkers = false)
        {
            MarkerCluster = await MarkerClustering.CreateAsync(jsRuntime, map1, await GetMarkersAsync());
            if (fitMarkers) await MarkerCluster.FitMapToMarkers(0);
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


        protected MarkerOptions GetClimbMarker(UserClimb climb, Map map)
        {
            var start = Columbae.Polyline.ParsePolyline(climb.Polyline).Vertices.First();
            var options = new MarkerOptions()
            {
                Position = new LatLngLiteral(start.Longitude, start.Latitude),
                Map = map,
                Icon = new Icon {Url = climb.Done ? "images/climb-icon-map-green.png": "images/climb-icon-map-pink.png"},
                Clickable = true,
                Title = climb.Name,
                Visible = true
            };

            return options;
        }


        protected async Task ZoomToClimbAsync(Map map1, Columbae.Polyline polyline)
        {
            if (map1 != null)
            {
                var box = polyline.BoundingBox;
                var bounds = new LatLngBoundsLiteral(
                    new LatLngLiteral(box.Vertices.First().Longitude, box.Vertices.First().Latitude),
                    new LatLngLiteral(box.Vertices[2].Longitude, box.Vertices[2].Latitude));
                await map1.FitBounds(bounds);
            }
        }


        public async Task<InfoWindow> CreateInfoWindowAsync(ClimbData climb, IJSRuntime jsRuntime,
            Polypoint position = null)
        {
            if (position == null)
            {
                position = Columbae.Polyline.ParsePolyline(climb.Polyline).Vertices.First();
            }

            var infoWindowContent = $@"<table class='table table-striped table-dark'><tbody>
                        <tr>
                        <td colspan=2>{climb.Name}</td>
                        </tr>
                        <tr>
                        <td>Points</td>
                        <td style='text-align: right'>{climb.CotacolPoints}</td>
                        </tr>
                        <tr>
                        <td>Length</td>
                        <td style='text-align: right'>{climb.Distance.Number()} m</td>
                        </tr>
                        <tr>
                        <td>Elevation</td>
                        <td style='text-align: right'>{climb.ElevationDiff.Number()}</td>
                        </tr>
                        </tbody>
                        </table>";
            var infoWindow = await InfoWindow.CreateAsync(jsRuntime, new InfoWindowOptions()
            {
                Position = new LatLngLiteral(position.Longitude, position.Latitude)
            });
            await infoWindow.SetContent(infoWindowContent);
            //await infoWindow.SetPosition(new LatLngLiteral(position.Longitude, position.Latitude));
            return infoWindow;
        }
    }
}