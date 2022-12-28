using GoogleMapsComponents;
using GoogleMapsComponents.Maps;

namespace Cotacol.Website.Services.Extensions
{
    public static class MapExtensions
    {
        public static LatLngBoundsLiteral GetMapBounds(this Columbae.Polyline polyline)
        {
            var box = polyline.BoundingBox;
            var bounds = new LatLngBoundsLiteral(
                new LatLngLiteral(box.Vertices.First().Longitude, box.Vertices.First().Latitude),
                new LatLngLiteral(box.Vertices[2].Longitude, box.Vertices[2].Latitude));
            return bounds;
        }

        public static async Task CenterBelgiumAsync(this GoogleMap map)
        {
            await map.InteropObject.FitBounds(Columbae.World.GeoConstants.BelgiumRough.GetMapBounds());
        }
    }
}