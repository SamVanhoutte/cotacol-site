using Columbae;
using Cotacol.Website.Services.Maps;
using CotacolApp.Models;
using GoogleMapsComponents.Maps;
using Microsoft.JSInterop;
using Route = Columbae.World.Route;

namespace Cotacol.Website.Interfaces
{
    public interface IMapService
    {
        Task<MapOptions> GetLayoutAsync(bool miniView = false, double zoom = 9);
        Task ClusterMarkersAsync(Map map1, IJSRuntime jsRuntime, bool fitMarkers = false);

        Task ShowClimbAsync(Map map1, IJSRuntime jsRuntime, UserClimb climb,
            MapLayout mapLayout);

        Task PlotRoute(Map map1, IJSRuntime jsRuntime, Route route);
        Task ClearClimbsAsync();
        Task<InfoWindow> CreateInfoWindowAsync(ClimbData climb, IJSRuntime jsRuntime, Polypoint position=null);
        Task ShowClimbsAsync(Map map1, IJSRuntime jsRuntime, List<UserClimb> toList, MapLayout layout, Action<MouseEvent, string> handleClick = null);
        Route RouteToPlot {  set; }
    }
}