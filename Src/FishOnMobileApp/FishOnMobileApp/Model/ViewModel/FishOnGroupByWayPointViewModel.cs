using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishOn.Model.ViewModel
{
    public class FishOnGroupByWayPointViewModel : ObservableCollection<FishOn>
    {
        public FishOnGroupByWayPointViewModel(string wayPointName)
        {
            WayPointName = wayPointName;
        }

        public string WayPointName { get; set; }
        public string WayPointShortName
        {
            get
            {
                return WayPointName.Substring(0, 4);
            }
        }

        public static ObservableCollection<FishOnGroupByWayPointViewModel> MapToObservableCollection(List<WayPoint> wps)
        {
            if (wps == null || wps.Count == 0)
            {
                return null;
            }

            var result = new ObservableCollection<FishOnGroupByWayPointViewModel>();
            FishOnGroupByWayPointViewModel wpGroup = null;

            int id = 0;
            var orderedFish = wps.SelectMany(w => w.FishCaught).Distinct().OrderBy(f => f.WayPointId);

            foreach (var fish in orderedFish)
            {
                if (id != fish.WayPointId)
                {
                    id = fish.WayPointId;
                    wpGroup = new FishOnGroupByWayPointViewModel(fish.WayPoint.Name);
                    result.Add(wpGroup);
                }

                wpGroup.Add(fish);
            }

            return result;
        }
    }
}
