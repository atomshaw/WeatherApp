using OpenWeatherMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap.Interface
{
    public interface IWeatherProvider
    {
        public Task<WeatherResponse> GeoSearch(string input);
        public Task<WeatherResponse> GeoReverseLatLonLookup(string input);
        public Task<WeatherResponse> GeoPostCode(string input);
        public Task<WeatherResponse> GetWeatherByCoordinates(GeoCodedResponse response);
    }
}
