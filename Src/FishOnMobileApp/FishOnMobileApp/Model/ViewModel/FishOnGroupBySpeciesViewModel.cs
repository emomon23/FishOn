using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FishOn.Model.ViewModel
{
    public class FishOnGroupBySpeciesViewModel : ObservableCollection<FishOn>
    {
        public FishOnGroupBySpeciesViewModel(string speciesName)
        {
            SpeciesGroupName = speciesName;
        }

        public string SpeciesGroupName { get; private set; }

        public string SpeciesGroupShortName
        {
            get
            {
                return SpeciesGroupName.Substring(0, 4);
            }
        }

        public static ObservableCollection<FishOnGroupBySpeciesViewModel> MapToObservableCollection(List<WayPoint> wps)
        {
            if (wps == null || wps.Count == 0)
            {
                return null;
            }

            var orderdFish = wps.SelectMany(wp => wp.FishCaught).Distinct().OrderBy(f => f.SpeciesId);
            var result = new ObservableCollection<FishOnGroupBySpeciesViewModel>();
         
            FishOnGroupBySpeciesViewModel speciesGroup=null;
            int id = 0;
            foreach (var fish in orderdFish)
            {
                if (fish.SpeciesId != id)
                {
                    speciesGroup = new FishOnGroupBySpeciesViewModel(fish.Species.Name);
                    result.Add(speciesGroup);
                    id = fish.SpeciesId;
                }

                speciesGroup.Add(fish);
            }

            return result;
        }
       
    }
}
