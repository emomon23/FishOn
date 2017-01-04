using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace FishOn.Model
{
    public class WeatherCondition
    {
        [PrimaryKey]
        public int WeatherConditionId { get; set; }
        public double AirTemp { get; set; }
        public double BerometricPressure { get; set; }
        public double HumidityPercent { get; set; }
        public double WindSpeed { get; set; }
        public double DewPoint { get; set; }
        public double Visibility { get; set; }
        public string Moon_Label { get; set; }
        public double Moon_Age { get; set; }
        public int Moon_IlluminationPercent { get; set; }
    }
}
