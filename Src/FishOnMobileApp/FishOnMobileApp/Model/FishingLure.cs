using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;


namespace FishOn.Model
{
    public class FishingLure
    {
        [PrimaryKey, AutoIncrement]
        public int FishingLureId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public double Size { get; set; }
        public double Weight { get; set; }

        [Ignore]
        public FishOn FishCaught { get; set; }
    }
}
