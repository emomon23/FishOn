using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using FishOn.Model;
using FishOn.Repositories;
using FishOn.Utils;

namespace FishOn.Services
{
    public interface IWeatherService
    {
        Task<WeatherCondition> GetWeatherConditionsAsync(double latitude, double longitude);
    }

    public class WeatherService : IWeatherService
    {
        private IFishOnHttpRepository _httpRepo;
        private IMoonPhaseParsingService _moonPhaseParsingService;

	    //Example: http://forecast.weather.gov/MapClick.php?lat=45.01058&lon=-93.4555&FcstType=dwml
        private const string _BaseUrl = "http://forecast.weather.gov/MapClick.php?lat=[LATITUDE]&lon=[LONGITUDE]&FcstType=dwml";

        public WeatherService()
        {
            _httpRepo = new FishOnHttpRepository();
            _moonPhaseParsingService = new MoonPhaseParsingService();
        }

        public WeatherService(IFishOnHttpRepository httpRepository, IMoonPhaseParsingService moonPhaseParsingService)
        {
            _httpRepo = httpRepository;
            _moonPhaseParsingService = moonPhaseParsingService;
        }

        public async Task<WeatherCondition> GetWeatherConditionsAsync(double latitude, double longitude)
        {
            //dwml/data [@type='current observations']/parameters
            // temp: /temperature[@type='apparent']/value
            // dew point: /temperature[@type='dew point']/value
            // humidity: /humidity/value
            // visibility: /wether/weather-conditions/value/visibility
            // wind speed: /wind-speed[@type='sustained']/value (note: value is in knots.  conversion: Knot * 1.152 = MPH)
            // barometer: /presure[@type='barometer']/value;

            const string BaseXPathQuery = "data[@type='current observations']/parameters";
            var url = _BaseUrl.Replace("[LATITUDE]", latitude.ToString()).Replace("[LONGITUDE]", longitude.ToString());
            var xRootElement = await _httpRepo.GetXMLAsync(url);
            
            WeatherCondition result = new WeatherCondition();
            result.AirTemp = xRootElement.GetValue($"{BaseXPathQuery}/temperature[@type='apparent']/value").ToDouble();
            result.DewPoint = xRootElement.GetValue($"{BaseXPathQuery}/temperature[@type='dew point']/value").ToDouble();
            result.HumidityPercent = xRootElement.GetValue($"{BaseXPathQuery}/humidity/value").ToDouble();
            result.WindSpeed = xRootElement.GetValue($"{BaseXPathQuery}/wind-speed[@type='sustained']/value").ToDouble();
            result.BerometricPressure = xRootElement.GetValue($"{BaseXPathQuery}/pressure[@type='barometer']/value").ToDouble();

            //weather conditions is a special hard case
            var weatherConditionsElements = xRootElement.XPathQuery($"{BaseXPathQuery}/weather")?.Elements("weather-conditions");
            if (weatherConditionsElements != null && weatherConditionsElements.Count() > 1)
            {
                result.Visibility = weatherConditionsElements.ElementAt(1).XPathQuery("value/visibility").Value.ToDouble();
            }

            var moonPhase = await _moonPhaseParsingService.GetCurrentMoonPhaseAsync();
            result.Moon_Age = moonPhase.Age;
            result.Moon_IlluminationPercent = moonPhase.IlluminationPercent;
            result.Moon_Label = moonPhase.Label;
            
            return result;
        }

        
       
    }

}
