using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.ModelView;
using FishOn.PlatformInterfaces;
using FishOn.Repositories;
using FishOn.Services;
using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.Settings
{
    public class SettingsListViewModel : BaseModelView
    {
        private ProvisioningPageDefinitions _provisioningPagesDefinitions;

        public SettingsListViewModel(INavigation navigation, ILakeDataService lakeDataService,
            ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService,
            IFishOnCurrentLocationService fishOnCurrentLocationService, IAppSettingService appSettingService,
            IFishCaughtDataService fishCaughtDataService, ISessionDataService sessionDataService)
            : base(
                navigation, lakeDataService, speciesDataService, wayPointDataService, fishOnCurrentLocationService,
                appSettingService, fishCaughtDataService, sessionDataService)
        {
            _provisioningPagesDefinitions = new ProvisioningPageDefinitions();
        }

        public List<ProvisioningPageDefinition> PageDefinitions
        {
            get { return _provisioningPagesDefinitions.GetList(); }
        }

        public async Task DisplayProvisioningPage(string pageTitle)
        {
            var pageDefinition = _provisioningPagesDefinitions.GetDefinition(pageTitle);
            var page = await pageDefinition.CreatePage(_navigation, _lakeService, _speciesDataService, _wayPointDataService,
                _locationService, _appSettingService, _fishOnDataService, _sessionDataService);

            await _navigation.PushAsync(page);

        }

    }
}
