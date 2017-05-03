using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FishOn.ModelView;
using FishOn.Pages_MVs.LakeMap;
using FishOn.Pages_MVs.ProvisioningPages.MyFish;
using FishOn.Pages_MVs.ProvisioningPages.Settings;
using FishOn.Pages_MVs.WaypointNameMethodRecorder;
using FishOn.PlatformInterfaces;
using FishOn.ProvisioningPages.WayPoints;
using Xamarin.Forms;

namespace FishOn.Services
{
    public class FishOnNavigationService
    {
        private INavigation _navigation = null;
        private IFishOnService _fishOnService;
        private AppSettingService _appSettingService;
        private ISessionDataService _sessionDataService;
        private WeatherService _weatherService;
        private IFishOnCurrentLocationService _currentLocationService =
            DependencyService.Get<IFishOnCurrentLocationService>();

      
        public FishOnNavigationService(INavigation navigation)
        {
            _sessionDataService = new SessionDataService();
            _navigation = navigation;
            _fishOnService = new FishOnService();
            _appSettingService = new AppSettingService();
            _weatherService = new WeatherService(_sessionDataService);
            
        }

        public async Task<MainPageModelView> CreateMainPageViewModel(INavigation xamarinNavigation)
        {
            MainPageModelView vm = new MainPageModelView(this, _fishOnService, _sessionDataService, _currentLocationService);
            await vm.InitializeAsync();
            return vm;
        }


        public async Task Navigate_ToLakeListAsync()
        {
            var viewModel = new LakeListModelView(this, _fishOnService, _sessionDataService);
            await viewModel.InitializeAsync();
            var lakesPage = new LakeListPage(viewModel);

            await _navigation.PushAsync(lakesPage);
        }


        public async Task Navigate_ToSpeciesListAsync()
        {
            var viewModel = new SpeciesPageModelView(this, _fishOnService);
            await viewModel.Initialize();

            var speciesPage = new SpeciesListPage(viewModel);

            await _navigation.PushAsync(speciesPage);
        }

        public async Task Naviage_ToLakeMapAsync()
        {
            var viewModel = new LakeMapPageModelView(this, _fishOnService, _currentLocationService);
            await viewModel.InitializeAsync();
            var lakeMapPage = new LakeMapMasterDetailPage(viewModel);
          
            await _navigation.PushAsync(lakeMapPage);

        }

        public async Task Navigate_BackToLandingPageAsync(string displayMessage = null)
        {
            await _navigation.PopToRootAsync(true);
        }


        public async Task Navigate_ToMyDataButtonsListAsync()
        {
            var vm = new SettingsListViewModel(this, _fishOnService);
            await vm.InitializeAsync();
            var page = new SettingsListPage(vm);

            await _navigation.PushAsync(page);
            
        }

        public async Task Naviage_ToWayPointProvisioningDetailPage()
        {
            
            var vm = new WayPointProvisioningModelView(this, _fishOnService);
            vm.InitializeAsync();

            var detailPage = new WPDetailPage(vm);
            await _navigation.PushAsync(detailPage);
            
        }

        public async Task Navigate_ToFishOnProvisioningPage()
        {
            var detailPage = new MyFishDetailPage()
            {
                BindingContext = this
            };
            await _navigation.PushAsync(detailPage);
        }

        public async Task Navigate_ToVoiceToTextPage(string selectedSpeciesName)
        {
            var species = (await _fishOnService.GetSpeciesListAsync()).Where(s => s.Name == selectedSpeciesName);
            var vm = new WaypointNameMethodRecorderViewModel(this, _fishOnService, _weatherService, _sessionDataService, _currentLocationService);
            vm.SelectedSpecies = species.FirstOrDefault();
            await vm.InitializeAsync();

            var page = new VoiceToTextPage(vm);
            await _navigation.PushAsync(page);
        }

        public async Task PushModalAsync(Page page)
        {
            await _navigation.PushModalAsync(page);
        }

        public async Task PopModalAsync()
        {
            await _navigation.PopModalAsync();
        }

        public async Task PopAsync()
        {
            await _navigation.PopAsync();
        }

        public async Task PushAsync(Page page)
        {
            await _navigation.PushAsync(page);
        }

    }
}
