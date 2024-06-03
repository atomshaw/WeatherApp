using NetTopologySuite.Geometries;

namespace OpenWeatherMap.Models
{
    public class GeoCodedResponse
    {
        public string Name { get; set; }
        public Point Coordinates { get; set; }
    }
}
