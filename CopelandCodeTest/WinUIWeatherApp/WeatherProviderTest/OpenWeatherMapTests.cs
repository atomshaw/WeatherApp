using OpenWeatherMap.Models;

namespace WeatherProviderTest
{
    public class OpenWeatherMapTests
    {
        private readonly OpenWeatherMap.OpenWeatherMap _weatherProvider;

        public OpenWeatherMapTests()
        {
            _weatherProvider = new OpenWeatherMap.OpenWeatherMap();
        }

        [Fact]
        public async Task GeoSearch_ValidInput_ReturnsWeatherResponse()
        {
            // Arrange
            string input = "New York";

            // Act
            var result = await _weatherProvider.GeoSearch(input);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WeatherResponse>(result);
        }
        [Fact]
        public async Task GeoReverseLatLonLookup_ValidInput_ReturnsWeatherResponse()
        {
            // Arrange
            string input = "40.7128, -74.0060";

            // Act
            var result = await _weatherProvider.GeoReverseLatLonLookup(input);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WeatherResponse>(result);
        }

        [Fact]
        public async Task GeoPostCode_ValidInput_ReturnsWeatherResponse()
        {
            // Arrange
            string input = "10001";

            // Act
            var result = await _weatherProvider.GeoPostCode(input);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WeatherResponse>(result);
        }

        [Fact]
        public async Task GeoPostCode_NonValidInput_ReturnsNull()
        {
            // Arrange
            string input = "1000155555555";

            // Act
            var result = await _weatherProvider.GeoPostCode(input);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GeoReverseLatLonLookup_NonValidInput_ReturnsNull()
        {
            // Arrange
            string input = "-77,";

            // Act
            var result = await _weatherProvider.GeoPostCode(input);

            // Assert
            Assert.Null(result);
        }

    }
}
