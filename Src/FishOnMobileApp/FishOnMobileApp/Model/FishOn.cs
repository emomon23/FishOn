using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishOn.Model
{
    public class FishOn
    {
        public int FishOnId { get; set; }
        public string Species { get; set; }
        public DateTime DateCaught { get; set; }
        public string Note { get; set; }
        public int WaterTemp { get; set; }
        public WeatherCondition WeatherCondition { get; set; }
        public int WayPointId { get; set; }

        public WayPoint WayPoint { get; set; }
    }
}
