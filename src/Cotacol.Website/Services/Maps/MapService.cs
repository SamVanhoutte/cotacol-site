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
                Center = new LatLngLiteral( 50.28, 4.52),
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
            // TODO : fitmap to markers?
            // if (fitMarkers) await MarkerCluster.  MarkerCluster.FitMapToMarkers(0);
        }

        private static async Task<MapTypeStyle[]> GetMapStylesAsync()
        {
            var style = new GoogleMapStyleBuilder();
            var assembly = Assembly.GetAssembly(typeof(MapService));
            var resourceName = "Cotacol.Website.StaticData.cotacolmapstyle.json";
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
                Position = new LatLngLiteral(start.Latitude, start.Longitude),
                Map = map,
                Icon = new Icon {Url = climb.Done ? "images/climb-icon-map-green.svg": "images/climb-icon-map-pink.svg"},
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
                    new LatLngLiteral(box.Vertices.First().Latitude, box.Vertices.First().Longitude),
                    new LatLngLiteral(box.Vertices[2].Latitude, box.Vertices[2].Longitude));
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
                Position = new LatLngLiteral(position.Latitude, position.Longitude)
            });
            await infoWindow.SetContent(infoWindowContent);
            //await infoWindow.SetPosition(new LatLngLiteral(position.Longitude, position.Latitude));
            return infoWindow;
        }
    }
}