using CoderPro.OpenWeatherMap.Wrapper;
using NetTopologySuite.Geometries;
using OpenWeatherMap.Models;
using System.Threading.Tasks;
using System;
using CoderPro.OpenWeatherMap.Wrapper.Models.CurrentWeather;
using OpenWeatherMap.Interface;


namespace OpenWeatherMap
{
    public class OpenWeatherMap : IWeatherProvider
    {
        string apiKey = "eb85dcf61ed99b245c4d8c24bbab241b";


        public async Task<WeatherResponse> GeoSearch(string input)
        {
            var geoCodeClient = new GeoCodingClient(apiKey);
            try
            {
                var res = await geoCodeClient.QueryCoordinatesAsync(input);
                var gcr = new GeoCodedResponse { Name = res.LocationList[0].Name, Coordinates = res.LocationList[0].Coordinates };
                return await GetWeatherByCoordinates(gcr);
            }
            catch
            {
                return null;
            }
        }
        public async Task<WeatherResponse> GeoReverseLatLonLookup(string input)
        {
            var latlon = input.Split(',');
            if (latlon.Length != 2) { return null; }
            var geoCodeClient = new GeoCodingClient(apiKey);
            try
            {
                var point = new Point(new Coordinate(Double.Parse(latlon[0].Trim()), Double.Parse(latlon[1].Trim())));
                var res = await geoCodeClient.QueryReverseAsync(point);
                var gcr = new GeoCodedResponse { Name = res.LocationList[0].Name, Coordinates = res.LocationList[0].Coordinates };
                return await GetWeatherByCoordinates(gcr);
            }
            catch
            {
                return null;
            }

        }

        public async Task<WeatherResponse> GeoPostCode(string input)
        {
            var geoCodeClient = new GeoCodingClient(apiKey);
            try
            {
                var res = await geoCodeClient.QueryCoordinatesByPostCodeAsync(input);
                var gcr = new GeoCodedResponse { Name = res.Name, Coordinates = res.Coordinate };
                return await GetWeatherByCoordinates(gcr);
            }
            catch
            {
                return null;
            }
        }

        public async Task<WeatherResponse> GetWeatherByCoordinates(GeoCodedResponse response)
        {
            var openWeatherClient = new CurrentWeatherClient(apiKey);
            var res = await openWeatherClient.QueryByCoordinatesAsync(SearchType.Coordinate, response.Coordinates);
            return new WeatherResponse
            {
                CityName = res.Name,
                CurrentTemp = res.Main.Temperature.FahrenheitCurrent,
                FeelsLikeTemp = res.Main.Temperature.FahrenheitFeelsLike,
                Humidity = res.Main.Humidity.ToString(),
                Pressure = res.Main.Pressure.ToString(),
                WeatherDescription = res.WeatherList[0].Description ?? "",
                IconURL = $"https://openweathermap.org/img/wn/{res.WeatherList[0].Icon}@2x.png"

            };
        }
    }
}
