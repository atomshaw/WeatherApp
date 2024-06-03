using Microsoft.UI.Xaml;
using System;
using CoderPro.OpenWeatherMap.Wrapper;
using System.Threading.Tasks;
using CoderPro.OpenWeatherMap.Wrapper.Models.CurrentWeather;
using Microsoft.UI.Xaml.Media.Imaging;
using NetTopologySuite.Geometries;
using Microsoft.UI.Xaml.Controls;
using NetTopologySuite.Utilities;

namespace WeatherApp
{
    public sealed partial class MainWindow : Window
    {
        string apiKey = "eb85dcf61ed99b245c4d8c24bbab241b";
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            GetWeather(this.Search.Text);
        }

        private async void GetWeather(string query)
        {
            var geoCodeResponse = await ParseInputAndGeoCode(query);
            if (geoCodeResponse != null)
            {
                var openWeatherClient = new CurrentWeatherClient(apiKey);
                var result = await openWeatherClient.QueryByCoordinatesAsync(SearchType.Coordinate, geoCodeResponse.Coordinates);
                PopulateUI(result);
            }
            else
            {
                ShowErrorMessage();
                ClearUI();
            }
        }

        private async void ShowErrorMessage()
        {
            await this.errorDialog.ShowAsync();
        }

        public async Task<GeoCodedResponse> ParseInputAndGeoCode(string input)
        {
            if (input.Contains(','))
            {
                return await GeoReverseLatLonLookup(input);
            }
            if (Double.TryParse(input, out _))
            {
                return await GeoPostCode(input);
            }
            return await GeoSearch(input);
        }
        private async Task<GeoCodedResponse> GeoSearch(string input)
        {
            var geoCodeClient = new GeoCodingClient(apiKey);
            try
            {
                var res = await geoCodeClient.QueryCoordinatesAsync(input);
                return new GeoCodedResponse { Name = res.LocationList[0].Name, Coordinates = res.LocationList[0].Coordinates };
            }
            catch
            {
                return null;
            }
        }

        private async Task<GeoCodedResponse> GeoPostCode(string input)
        {
            var geoCodeClient = new GeoCodingClient(apiKey);
            try
            {
                var res = await geoCodeClient.QueryCoordinatesByPostCodeAsync(input);
                return new GeoCodedResponse { Name = res.Name, Coordinates = res.Coordinate };
            }
            catch
            {
                return null;
            }
        }

        private async Task<GeoCodedResponse> GeoReverseLatLonLookup(string input)
        {
            var latlon = input.Split(',');
            if (latlon.Length != 2) { return null; }
            var geoCodeClient = new GeoCodingClient(apiKey);
            try
            {
                var point = new Point(new Coordinate(Double.Parse(latlon[0].Trim()), Double.Parse(latlon[1].Trim())));
                var res = await geoCodeClient.QueryReverseAsync(point);
                return new GeoCodedResponse { Name = res.LocationList[0].Name, Coordinates = res.LocationList[0].Coordinates };
            }
            catch
            {
                return null;
            }

        }

        private void PopulateUI(QueryResponse response)
        {
            try
            {
                var iconSrc = $"https://openweathermap.org/img/wn/{response.WeatherList[0].Icon}@2x.png";
                WeatherIcon.Source = new BitmapImage(new Uri(iconSrc));
            }
            catch 
            {
               //Can't find image icon, move on
            }
            var temperature = string.Format("{0:N0} °F", (int)response.Main.Temperature.FahrenheitCurrent);
            var feelslike = string.Format("{0:N0} °F", (int)response.Main.Temperature.FahrenheitFeelsLike);
            this.CityName.Text = "City (Observed): " + response.Name;
            this.WeatherTemp.Text = "Temperature: " +  temperature;
            this.WeatherFeelsLike.Text = "Feels Like: " + feelslike;
            this.WeatherDescription.Text = "Weather Description: " + response.WeatherList[0].Description ?? "";
            this.WeatherHumidity.Text = "Humidity: " + response.Main.Humidity.ToString() + "%" ?? "";
            this.WeatherPressure.Text = "Pressure: " + response.Main.Pressure.ToString() + " hPa" ?? "";
        }

        private void ClearUI()
        {
            WeatherIcon.Source = null;
            this.CityName.Text = string.Empty;
            this.WeatherTemp.Text = string.Empty;
            this.WeatherFeelsLike.Text = string.Empty;
            this.WeatherDescription.Text = string.Empty;
            this.WeatherHumidity.Text = string.Empty;
            this.WeatherPressure.Text = string.Empty;
            this.Search.Text = string.Empty;
        }

        public class GeoCodedResponse
        {
            public string Name { get; set; }
            public Point Coordinates { get; set; }
        }


    }
}
