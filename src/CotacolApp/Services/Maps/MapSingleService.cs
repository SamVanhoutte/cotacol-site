using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CotacolApp.Models;
using GoogleMapsComponents.Maps;
using Microsoft.JSInterop;

namespace CotacolApp.Services.Maps
{
    public class MapSingleService : MapService, IMapService
    {
        private List<Marker> _markers = new List<Marker>();
        private List<Polyline> _lines = new();

        protected override Task<List<Marker>> GetMarkersAsync()
        {
            return Task.FromResult(_markers);
        }
        
        public async Task ShowClimbAsync(Map map1, IJSRuntime jsRuntime, UserClimb climb,
            MapLayout mapLayout)
        {
            if (!string.IsNullOrEmpty(climb.Polyline))
            {
                var polyline = Columbae.Polyline.ParsePolyline(climb.Polyline);

                if (!mapLayout.ShowPolylines) await AddMarkerAsync(map1, jsRuntime, climb, polyline, mapLayout.SingleClimb);
                if (mapLayout.ShowPolylines) await DrawPolylineAsync(map1, jsRuntime, climb, polyline, true);
                if (mapLayout.SingleClimb) await ZoomToClimbAsync(map1, polyline);
            }
        }
        
        public async Task ShowClimbsAsync(Map map1, IJSRuntime jsRuntime, List<UserClimb> climbList, MapLayout mapLayout)
        {
            if (climbList != null && map1 != null)
            {
                await ClearClimbsAsync();
                var tasks = new List<Task> { };
                tasks.AddRange(climbList.Select(
                    userClimb => ShowClimbAsync(map1, jsRuntime, userClimb, mapLayout)));
                await Task.WhenAll(tasks);

                //await _mapService.ShowClimbsAsync(map1, JsRuntime, climbList);
                if (!mapLayout.ShowPolylines)
                {
                    await ClusterMarkersAsync(map1, jsRuntime, false);
                }
            }
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

            if (MarkerCluster != null)
            {
                await MarkerCluster.ClearMarkers();
                await MarkerCluster.SetMap(null);
            }

            _markers = new List<Marker>();
            _lines = new List<Polyline>();
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
            await marker.AddListener("click", async () => { await infoWindow.Open(map1); });
            _markers.Add(marker);
        }
        
        private async Task DrawPolylineAsync(Map map1, IJSRuntime jsRuntime, ClimbData climb,
            Columbae.Polyline polyline,
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
                await line.AddListener("click", async () => { await infoWindow.Open(map1); });
            }

            _lines.Add(line);
        }
    }
}