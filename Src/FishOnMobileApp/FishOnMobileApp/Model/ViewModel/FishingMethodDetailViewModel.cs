using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishOn.Model.ViewModel
{
    public class FishingMethodDetailViewModel
    {
        public int WayPointId { get; set; }
        public string WayPointName { get; set; }
        public List<FishingMethodDayResult> MethodHistory { get; set; }
    }

    public class FishingMethodDayResult
    {
        public string Date { get; set; }
        public Dictionary<string, int> SpeciesCount { get; set; }
        public string MoonPhaseThisDay { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
