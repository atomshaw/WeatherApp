using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using System.Threading.Tasks;
using WeatherApp;
using NetTopologySuite.Geometries;



namespace WeatherAppTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(0, 0);
        }

        // Use the UITestMethod attribute for tests that need to run on the UI thread.
        [UITestMethod]
        public void TestMethod2()
        {
            var grid = new Grid();
            Assert.AreEqual(0, grid.MinWidth);
        }

        [UITestMethod]
        public async Task ParseInputAndGeoCode_ValidLatLon_ReturnsGeoCodedResponse()
        {
            // Arrange
            var mainWindow = new MainWindow();
            string input = "40.730610, -73.935242"; // New York City

            // Act
            var result = await mainWindow.ParseInputAndGeoCode(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("New York", result.CityName);
        }
    }
}
