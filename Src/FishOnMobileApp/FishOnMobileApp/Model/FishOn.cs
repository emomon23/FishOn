using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLite.Net.Attributes;

namespace FishOn.Model
{
    public class FishOn
    {
        [PrimaryKey, AutoIncrement]
        public int FishOnId { get; set; }
        public string Species { get; set; }
        public DateTime DateCaught { get; set; }
        public string Note { get; set; }
        public int? WaterTemp { get; set; }

        [Indexed]
        public int? FishingLureId { get; set; }

        [Indexed]
        public int WayPointId { get; set; }
        public WeatherCondition WeatherCondition { get; set; }
       
        public virtual FishingLure Lure { get; set; }
        public virtual WayPoint WayPoint { get; set; }
    }
}
