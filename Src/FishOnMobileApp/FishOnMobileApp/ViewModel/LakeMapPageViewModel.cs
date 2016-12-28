using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Services;
using Xamarin.Forms;

namespace FishOn.ViewModel
{
    public class LakeMapPageViewModel : BaseViewModel
    {
        public LakeMapPageViewModel(INavigation navigation) : base(navigation) { }
        public LakeMapPageViewModel(INavigation navigation, ILakeDataService lakeDataService, ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService):base(navigation, lakeDataService, speciesDataService, wayPointDataService) { }

    }
}
