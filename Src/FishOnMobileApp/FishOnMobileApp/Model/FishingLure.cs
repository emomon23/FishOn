using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Utils;
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
        public string Method { get; set; }
        public string Note { get; set; }

        public string LureDescriptionSummary
        {
            get { return $"{Name} {Color} {Size} {Weight}"; }
        }

        public bool IsValid
        {
            get { return Name.IsNotNullOrEmpty(); }
        }

        [Ignore]
        public FishOn FishCaught { get; set; }

      
    }
}
