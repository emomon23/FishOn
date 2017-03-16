using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.ModelView;
using FishOn.PlatformInterfaces;
using FishOn.Services;
using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.Settings
{
    public class SettingsListViewModel : BaseModelView
    {
        public SettingsListViewModel(INavigation navigation, ILakeDataService lakeDataService,
            ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService,
            IFishOnCurrentLocationService fishOnCurrentLocationService, IAppSettingService appSettingService,
            IFishCaughtDataService fishCaughtDataService)
            : base(
                navigation, lakeDataService, speciesDataService, wayPointDataService, fishOnCurrentLocationService,
                appSettingService, fishCaughtDataService)
        {
        }



    }
}
