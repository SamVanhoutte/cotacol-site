using System.Collections.Generic;
using System.Threading.Tasks;
using CotacolApp.Models;
using CotacolApp.Services.Maps;
using GoogleMapsComponents.Maps;
using Microsoft.JSInterop;

namespace CotacolApp.Services
{
    public interface IMapService
    {
        Task<MapOptions> GetLayoutAsync(bool miniView = false, int zoom = 9);
        Task ClusterMarkersAsync(Map map1, IJSRuntime jsRuntime, bool fitMarkers = false);

        Task ShowClimbAsync(Map map1, IJSRuntime jsRuntime, UserClimb climb,
            MapLayout mapLayout);

        Task ShowClimbsAsync(Map map1, IJSRuntime jsRuntime, List<UserClimb> climb,
            MapLayout mapLayout);

        Task ClearClimbsAsync();
    }
}