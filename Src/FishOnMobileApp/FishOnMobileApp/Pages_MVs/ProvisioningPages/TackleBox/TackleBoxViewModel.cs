using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.PlatformInterfaces;
using FishOn.Services;
using FishOn.ViewModel;
using Xamarin.Forms;

namespace FishOn.Pages_VMs.ProvisioningPages.TackleBox
{
    public class TackleBoxViewModel : BaseViewModel
    {
        public TackleBoxViewModel(INavigation navigation, ILakeDataService lakeDataService,
           ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService, IFishOnCurrentLocationService locationService, IAppSettingService appSettingService)
            : base(navigation, lakeDataService, speciesDataService, wayPointDataService, locationService, appSettingService){ }

    }
}
