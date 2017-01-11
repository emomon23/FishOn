using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FishOn.Pages_MVs.LakeMap;
using FishOn.Pages_MVs.MyData;
using FishOn.PlatformInterfaces;
using FishOn.ProvisioningPages.WayPoints;
using FishOn.Services;
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
        }

        public BaseModelView(INavigation navigation, ILakeDataService lakeDataService, ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService, IFishOnCurrentLocationService locationService, IAppSettingService appSettingService)
        {
            _navigation = navigation;
            _lakeService = lakeDataService;
            _speciesDataService = speciesDataService;
            _wayPointDataService = wayPointDataService;
            _locationService = locationService;
            _appSettingService = appSettingService;
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public virtual async Task InitializeAsync() { }

        protected void OnPropertyChanged([CallerMemberName] string name="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #region Navigation Methods

        protected async Task Navigate_ToLakeListAsync()
        {
            var lakesPage = new LakeListPage();
            var viewModel = new LakeListModelView(lakesPage.Navigation, _lakeService, _speciesDataService, _wayPointDataService, _locationService, _appSettingService);
            await viewModel.Initialize();
            lakesPage.BindingContext = viewModel;

            await _navigation.PushAsync(lakesPage);
        }

        protected async Task Navigate_ToSpeciesListAsync()
        {
            var speciesPage = new SpeciesListPage();
            var viewModel =  new SpeciesPageModelView(speciesPage.Navigation, _lakeService, _speciesDataService, _wayPointDataService, _locationService, _appSettingService);
            await viewModel.Initialize();
            speciesPage.BindingContext = viewModel;

            await _navigation.PushAsync(speciesPage);
        }

        protected async Task Naviage_ToLakeMapAsync()
        {
            var lakeMapPage = new LakeMapMasterDetailPage();
            var viewModel = new LakeMapPageModelView(lakeMapPage.Navigation, lakeMapPage.WayPointsMap, _lakeService, _speciesDataService, _wayPointDataService, _locationService, _appSettingService);
            await viewModel.InitializeAsync();
            lakeMapPage.BindingContext = viewModel;

            await _navigation.PushAsync(lakeMapPage);
            
        }

        protected async Task Navigate_BackToLandingPageAsync()
        {
            await _navigation.PopToRootAsync(true);
        }

        protected async Task Navigate_To_WayPointProvisoning_MasterDetailPageAsync()
        {
            var page = new WPProvisoningList();
            var viewModel = new WayPointProvisioningModelView(page.Navigation, _lakeService, _speciesDataService, _wayPointDataService, _locationService, _appSettingService);
            await viewModel.InitializeAsync();

            page.BindingContext = viewModel;
            await _navigation.PushAsync(page);
        }
        
        protected async Task Navigate_ToMyDataButtonsListAsync()
        {
           // Page page = (Page) Activator.CreateInstance(typeof(MyDataListPage));
            var page = new MyDataListPage();
            page.BindingContext =  new MyDataListModelView(page.Navigation, _lakeService, _speciesDataService, _wayPointDataService, _locationService, _appSettingService);

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
