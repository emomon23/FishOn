using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishOn.Model.ViewModel
{
    public class WayPointListItemViewModel
    {
        public int WayPointId { get; set; }
        public string WayPointName { get; set; }
        public int TotalFishCaught { get; set; }
        public string LakeName { get; set; }
        public string LastFished { get; set; }
    }
}
