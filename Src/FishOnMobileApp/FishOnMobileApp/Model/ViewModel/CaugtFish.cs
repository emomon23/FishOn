using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishOn.Model.ViewModel
{
    public class CaugtFish
    {
        private string dateTime = DateTime.Now.ToString("G");
        public string OriginalWayPointName { get; set; }
        public string OriginalMethod { get; set; }

        public string WayPointName { get; set; }
        public string FishingMethod { get; set; }

        public string DateCaught
        {
            get { return dateTime; }
            set { dateTime = value; }
        }

        public int WaterTemp { get; set; }

        public string MoonPhase { get; set; }

        public string Conditions { get; set; }

        public string LakeName { get; set; }

        public string Image1FileName { get; set; }
        public string Image2FileName { get; set; }
    }
}
