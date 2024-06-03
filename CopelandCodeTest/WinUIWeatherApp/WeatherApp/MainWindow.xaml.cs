using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using NetTopologySuite.Geometries;
using OpenWeatherMap.Interface;
using OpenWeatherMap.Models;
using System;
using System.Threading.Tasks;

namespace WeatherApp
{
    public sealed partial class MainWindow : Window
    {
        IWeatherProvider _provider =  new OpenWeatherMap.OpenWeatherMap();
        
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
            var weatherData = await ParseInputAndGeoCode(query);
            if (weatherData != null)
            {
                PopulateUI(weatherData);
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

        public async Task<WeatherResponse> ParseInputAndGeoCode(string input)
        {
            if (input.Contains(','))
            {
                return await _provider.GeoReverseLatLonLookup(input);
            }
            if (Double.TryParse(input, out _))
            {
                return await _provider.GeoPostCode(input);
            }
            return await _provider.GeoSearch(input);
        }
        private void PopulateUI(WeatherResponse response)
        {
            try
            {
                WeatherIcon.Source = new BitmapImage(new Uri(response.IconURL));
            }
            catch 
            {
               //Can't find image icon, move on
            }
            var temperature = string.Format("{0:N0} °F", (int)response.CurrentTemp);
            var feelslike = string.Format("{0:N0} °F", (int)response.FeelsLikeTemp);
            this.CityName.Text = "City (Observed): " + response.CityName;
            this.WeatherTemp.Text = "Temperature: " +  temperature;
            this.WeatherFeelsLike.Text = "Feels Like: " + feelslike;
            this.WeatherDescription.Text = "Weather Description: " + response.WeatherDescription ?? "";
            this.WeatherHumidity.Text = "Humidity: " + response.Humidity + "%" ?? "";
            this.WeatherPressure.Text = "Pressure: " + response.Pressure + " hPa" ?? "";
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
    }
}
