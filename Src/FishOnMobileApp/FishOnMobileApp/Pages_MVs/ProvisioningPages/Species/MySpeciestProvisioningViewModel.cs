using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.ModelView;
using FishOn.Pages_MVs.AccordionViewModel;
using FishOn.PlatformInterfaces;
using FishOn.Services;
using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.Species
{
    public class MySpeciestProvisioningViewModel : BaseModelView
    {
        public MySpeciestProvisioningViewModel(INavigation navigation) : base(navigation) { }

        public MySpeciestProvisioningViewModel(INavigation navigation, ILakeDataService lakeDataService,
            ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService,
            IFishOnCurrentLocationService locationService, IAppSettingService appSettingService, IFishCaughtDataService fishCaughtDataService)
            : base(
                navigation, lakeDataService, speciesDataService, wayPointDataService, locationService, appSettingService, fishCaughtDataService){}
    }
}
