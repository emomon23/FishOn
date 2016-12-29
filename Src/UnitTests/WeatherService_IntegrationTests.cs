using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FishOn.Services;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace UnitTests
{
    public class WeatherService_IntegrationTests
    {
        private double _latitude = 45.0105;
        private double _longitude = -93.4555;

        [Test]
        public async Task GetCurrentConditionsTest()
        {
            IWeatherService weatherService = new WeatherService();
            var currentConditions =await weatherService.GetWeatherConditionsAsync(_latitude, _longitude);

            Assert.IsNotNull(currentConditions);
            Assert.IsTrue(currentConditions.AirTemp != 0);
            Assert.IsTrue(currentConditions.BerometricPressure != 0);
            Assert.IsTrue(currentConditions.DewPoint != 0);
            Assert.IsTrue(currentConditions.HumidityPercent != 0);
            Assert.IsTrue(currentConditions.Visibility != 0);
            Assert.IsTrue(currentConditions.WindSpeed != 0);
      
        }
    }
}
