using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FishOn.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FishOn.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected readonly ILakeDataService _lakeService;
        protected readonly ISpeciesDataService _speciesDataService;
        protected readonly IWayPointDataService _wayPointDataService;
        protected readonly INavigation _navigation;
        private bool _isBusy = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public BaseViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _lakeService = new LakeDataService();
            _wayPointDataService = new WayPointDataService();
            _speciesDataService = new SpeciesDataService();
        }

        public BaseViewModel(INavigation navigation, ILakeDataService lakeDataService, ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService)
        {
            _navigation = navigation;
            _lakeService = lakeDataService;
            _speciesDataService = speciesDataService;
            _wayPointDataService = wayPointDataService;
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

        protected void OnPropertyChanged([CallerMemberName] string name="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #region Navigation Methods

        protected async Task Navigate_ToLakeListAsync()
        {
            var lakesPage = new LakeListPage();
            var viewModel = new LakeListViewModel(lakesPage.Navigation, _lakeService, _speciesDataService, _wayPointDataService);
            await viewModel.Initialize();
            lakesPage.BindingContext = viewModel;

            await _navigation.PushAsync(lakesPage);
        }

        protected async Task Navigate_ToSpeciesListAsync()
        {
            var speciesPage = new SpeciesListPage();
            var viewModel =  new SpeciesPageViewModel(speciesPage.Navigation, _lakeService, _speciesDataService, _wayPointDataService);
            await viewModel.Initialize();
            speciesPage.BindingContext = viewModel;

            await _navigation.PushAsync(speciesPage);
        }

        protected async Task Naviage_ToLakeMapAsync()
        {
            var lakeMapPage = new LakeMapPage();
            lakeMapPage.BindingContext = new LakeMapPageViewModel(lakeMapPage.Navigation, _lakeService, _speciesDataService, _wayPointDataService);

            await _navigation.PushAsync(lakeMapPage);
        }

        protected async Task Navigate_BackToLandingPageAsync()
        {
            await _navigation.PopToRootAsync(true);
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
