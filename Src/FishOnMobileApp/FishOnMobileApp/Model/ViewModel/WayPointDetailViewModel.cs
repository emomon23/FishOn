using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishOn.Model.ViewModel
{
    public class WayPointDetailViewModel
    {
        public int WayPointId { get; set; }
        public string WayPointName { get; set; }
        public List<FishingMethodSummaryViewModel> Methods { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class FishingMethodSummaryViewModel
    {
        public int MethodId { get; set; }
        public string MethodText { get; set; }
        public int TotalFishCaught { get; set; }
        public string DateLastFished { get; set; }
        public List<string> Images { get; set; }
    }
}
