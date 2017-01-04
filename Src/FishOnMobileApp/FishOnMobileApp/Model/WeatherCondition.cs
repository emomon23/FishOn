using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishOn.Model
{
    public class WeatherCondition
    {
        public double AirTemp { get; set; }
        public double BerometricPressure { get; set; }
        public double HumidityPercent { get; set; }
        public double WindSpeed { get; set; }
        public double DewPoint { get; set; }
        public double Visibility { get; set; }
        public MoonPhase MoonPhase { get; set; }
    }
}
