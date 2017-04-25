using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.Model;
using FishOn.Pages_MVs;
using FishOn.Pages_MVs.LakeMap;
using FishOn.Pages_MVs.MyData;
using FishOn.Pages_MVs.ProvisioningPages.Lakes;
using FishOn.Pages_MVs.ProvisioningPages.MyFish;
using FishOn.Pages_MVs.ProvisioningPages.Settings;
using FishOn.Pages_MVs.WaypointNameMethodRecorder;
using FishOn.PlatformInterfaces;
using FishOn.ProvisioningPages.WayPoints;
using FishOn.Repositories;
using FishOn.Services;
using FishOn.Utils;
using FishOnMobileApp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FishOn.ModelView
{
    public abstract class BaseModelView : INotifyPropertyChanged
    {
        protected readonly ILakeDataService _lakeService;
        protected readonly ISpeciesDataService _speciesDataService;
        protected readonly IWayPointDataService _wayPointDataService;
        protected readonly INavigation _navigation;
        protected readonly IFishOnCurrentLocationService _locationService;
        protected readonly IAppSettingService _appSettingService;
        protected readonly IFishCaughtDataService _fishOnDataService;
        protected readonly ISessionDataService _sessionDataService;

        private bool _isBusy = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public BaseModelView(INavigation navigation)
        {
            _navigation = navigation;
            _lakeService = new LakeDataService();
            _wayPointDataService = new WayPointDataService();
            _speciesDataService = new SpeciesDataService();
            _locationService = DependencyService.Get<IFishOnCurrentLocationService>();
            _appSettingService = new AppSettingService();
            _fishOnDataService = new FishCaughtDataService();
            _sessionDataService = new SessionDataService();
        }

        public BaseModelView(INavigation navigation, ILakeDataService lakeDataService, ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService, IFishOnCurrentLocationService locationService, IAppSettingService appSettingService, IFishCaughtDataService fishOnDataService, ISessionDataService sessionDataService)
        {
            _navigation = navigation;
            _lakeService = lakeDataService;
            _speciesDataService = speciesDataService;
            _wayPointDataService = wayPointDataService;
            _locationService = locationService;
            _appSettingService = appSettingService;
            _fishOnDataService = fishOnDataService;
            _sessionDataService = sessionDataService;
        }

        //for bindings
        public Color ButtonBackColor => StyleSheet.Button_BackColor;
        public Color ButtonTextColor => StyleSheet.Button_TextColor;
        public Color AccentColor => StyleSheet.NavigationPage_BarTextColor;

        public double HalfPageWidth => Application.Current.MainPage.Width / 2;
        public double HalfPageHeight => Application.Current.MainPage.Height / 2;

        public int Default_Label_Font_Size => StyleSheet.Default_Label_Font_Size;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        protected async Task<bool> AreYouSureAsync(string prompt)
        {
            var answer =  await App.Current.MainPage.DisplayAlert("Are you sure?",
                       prompt, "Yes", "No");

            return answer;
        }

        public virtual async Task InitializeAsync() { }

        protected void OnPropertyChanged([CallerMemberName] string name="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #region Navigation Methods

        protected async Task Navigate_ToLakeListAsync()
        {
            var viewModel = new LakeListModelView(_navigation, _lakeService, _speciesDataService, _wayPointDataService, _locationService, _appSettingService, _fishOnDataService, _sessionDataService);
            await viewModel.InitializeAsync();
            var lakesPage = new LakeListPage(viewModel);
         
            await _navigation.PushAsync(lakesPage);
        }

       
        protected async Task Navigate_ToSpeciesListAsync()
        {
            var viewModel = new SpeciesPageModelView(this._navigation, _lakeService, _speciesDataService, _wayPointDataService, _locationService, _appSettingService, _fishOnDataService, _sessionDataService);
            await viewModel.Initialize();

            var speciesPage = new SpeciesListPage(viewModel);
          
            await _navigation.PushAsync(speciesPage);
        }

        protected async Task Naviage_ToLakeMapAsync()
        {
            var lakeMapPage = new LakeMapMasterDetailPage();
            var viewModel = new LakeMapPageModelView(lakeMapPage.Navigation, lakeMapPage.WayPointsMap, _lakeService, _speciesDataService, _wayPointDataService, _locationService, _appSettingService, _fishOnDataService, _sessionDataService);
            await viewModel.InitializeAsync();
            lakeMapPage.BindingContext = viewModel;

            await _navigation.PushAsync(lakeMapPage);
            
        }

        protected async Task Navigate_BackToLandingPageAsync(string displayMessage = null)
        {
            await _navigation.PopToRootAsync(true);
        }

      
        protected async Task Navigate_ToMyDataButtonsListAsync()
        {
            var vm = new SettingsListViewModel(_navigation, _lakeService, _speciesDataService, _wayPointDataService, _locationService, _appSettingService, _fishOnDataService, _sessionDataService);
            var page = new SettingsListPage(vm);

            await _navigation.PushAsync(page);
        }

        protected async Task Naviage_ToWayPointProvisioningDetailPage()
        {
            var vm = (WayPointProvisioningModelView) this;
            var detailPage = new WPDetailPage(vm);
            await _navigation.PushAsync(detailPage);
        }

        protected async Task Navigate_ToFishOnProvisioningPage()
        {
            var detailPage = new MyFishDetailPage()
            {
                BindingContext = this
            };
            await _navigation.PushAsync(detailPage);
        }

        protected async Task Navigate_ToVoiceToTextPage()
        {
            var vm = new WaypointNameMethodRecorderViewModel(_navigation, _lakeService, _speciesDataService, _wayPointDataService, _locationService, _appSettingService, _fishOnDataService, _sessionDataService);
            await vm.InitializeAsync();
            var page = new VoiceToTextPage(vm);
            await _navigation.PushAsync(page);
        }

        #endregion
        
    }

    //Found this work around at: https://forums.xamarin.com/discussion/comment/105285/#Comment_105285
    //When a list view is bound to a collection and for each collection a element with a Command={Binding SomeCommand} is specified
    //The call won't call it on the parent viewmodel, it expects to find it on the context for the list item
    [ContentProperty("ElementName")]
    public class ElementSource : IMarkupExtension
    {
        public string ElementName { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            var rootProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            if (rootProvider == null)
                return null;
            var root = rootProvider.RootObject as Element;
            if (root == null)
                return null;
            return root.FindByName<Element>(ElementName);
        }
    }
}
