using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.PlatformInterfaces;
using FishOn.Services;
using FishOn.ModelView;
using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.TackleBox
{
    public class TackleBoxModelView : BaseModelView
    {
        public TackleBoxModelView(INavigation navigation, ILakeDataService lakeDataService,
           ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService, IFishOnCurrentLocationService locationService, IAppSettingService appSettingService, IFishCaughtDataService fishCaughtDataService)
            : base(navigation, lakeDataService, speciesDataService, wayPointDataService, locationService, appSettingService, fishCaughtDataService){ }

    }
}
