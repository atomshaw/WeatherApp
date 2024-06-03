using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap.Models
{
    public class WeatherResponse
    {
        public string IconURL { get; set; }//$"https://openweathermap.org/img/wn/{response.WeatherList[0].Icon}@2x.png"
        public string CityName { get; set; }
        public double CurrentTemp { get; set; } = 0;
        public double FeelsLikeTemp { get; set; } = 0;
        public string Humidity { get; set; }
        public string Pressure { get; set; }
        public string WeatherDescription { get; set; }
    }
}
