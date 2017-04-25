using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using FishOn.Model;
using FishOn.PlatformInterfaces;
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
        
	    //Example: http://forecast.weather.gov/MapClick.php?lat=45.01058&lon=-93.4555&FcstType=dwml
        private const string _BaseUrl = "http://forecast.weather.gov/MapClick.php?lat=[LATITUDE]&lon=[LONGITUDE]&FcstType=dwml";

        public WeatherService()
        {
            _httpRepo = new FishOnHttpRepository();
        }

        public WeatherService(IFishOnHttpRepository httpRepository)
        {
            _httpRepo = httpRepository;
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

            result.Moon_Age = GetMoonAge();
            result.Moon_IlluminationPercent = GetMoonIlliumination(result.Moon_Age);
            result.Moon_Label = GetMoonPhaseName(result.Moon_Age);
            
            return result;
        }
        
        private double GetMoonAge()
        {
            var now = DateTime.Now;
            return GetMoonAge(now.Day, now.Month, now.Year);
        }
        private double GetMoonAge(int d, int m, int y)
        {
            double age;
            int j = JulianDate(d, m, y);
            //Calculate the approximate phase of the moon
            var ip = (j + 4.867) / 29.53059;
            ip = ip - Math.Floor(ip);
            //After several trials I've seen to add the following lines, 
            //which gave the result was not bad 
            if (ip < 0.5)
                age = ip * 29.53059 + 29.53059 / 2;
            else
                age = ip * 29.53059 - 29.53059 / 2;
            // Moon's age in days
            age = Math.Floor(age) + 1;
            return age;
        }

        private int JulianDate(int d, int m, int y)
        {
            int mm, yy;
            int k1, k2, k3;
            int j;

            yy = y - (int)((12 - m) / 10);
            mm = m + 9;
            if (mm >= 12)
            {
                mm = mm - 12;
            }
            k1 = (int)(365.25 * (yy + 4712));
            k2 = (int)(30.6001 * mm + 0.5);
            k3 = (int)((int)((yy / 100) + 49) * 0.75) - 38;
            // 'j' for dates in Julian calendar:
            j = k1 + k2 + d + 59;
            if (j > 2299160)
            {
                // For Gregorian calendar:
                j = j - k3; // 'j' is the Julian date at 12h UT (Universal Time)
            }
            return j;
        }

        private string GetMoonPhaseName(double age)
        {
            if (age < 1)
                return "New Moon";

            if (age < 7)
                return "Waxing Cresent";

            if (age > 7 && age <= 8)
                return "1st Quarter";

            if (age > 8 && age <= 14)
                return "Waxing Gibbious";

            if (age > 14 && age <= 15)
                return "Full Moon";

            if (age > 15 && age < 21)
                return "Wanning Gibbous";

            if (age >= 21 && age <= 22)
                return "Last Quarter";

            if (age > 22 && age < 30)
                return "Wanning Cresent";

            return $"Unknown Age: {age}";
        }

        private int GetMoonIlliumination(double age)
        {
            const double peak = 14.78;
            const double max = 29.02;

            var temp = age <= peak ? age : max - age;
            return (int)((temp / peak) * (double)100);
        }


    }

}
