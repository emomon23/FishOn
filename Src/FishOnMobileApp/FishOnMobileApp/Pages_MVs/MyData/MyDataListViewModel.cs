using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.Pages_VMs.ProvisioningPages.Lakes;
using FishOn.Pages_VMs.ProvisioningPages.MyFish;
using FishOn.Pages_VMs.ProvisioningPages.Species;
using FishOn.Pages_VMs.ProvisioningPages.TackleBox;
using FishOn.PlatformInterfaces;
using FishOn.ProvisioningPages.WayPoints;
using FishOn.Services;
using FishOn.Utils;
using Xamarin.Forms;

namespace FishOn.ViewModel
{
    public class MyDataListViewModel : BaseViewModel
    {
        private List<Page> _pages = new List<Page>();

        public MyDataListViewModel(INavigation navigation) : base(navigation) { }
        public MyDataListViewModel(INavigation navigation, ILakeDataService lakeDataService, ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService, IFishOnCurrentLocationService locationService, IAppSettingService appSettingService):base(navigation, lakeDataService, speciesDataService, wayPointDataService, locationService, appSettingService) { }
       
        public string Title
        {
            get { return "Data List"; }
        }

        public async Task InitializeNewPageContextAsync(string pageTitle)
        {
            //Don't iniitalize every content page on the tab
            //wait until the user wants to view that content page.
            var p = _pages.FindPage(pageTitle);

            if (p != null && p.BindingContext != null)
            {
                var pageVM = (BaseViewModel) p.BindingContext;
                await pageVM.InitializeAsync();
            }
        }

        //Unable to bind the TabbedPage.Children to "{Binding ChildPages}"
        public List<Page> GetContentPages()
        {
            _pages = new List<Page>();

            _pages.Add(new WPProvisoningList()
            {
                BindingContext =
                    new WayPointProvisioningViewModel(_navigation, _lakeService, _speciesDataService,
                        _wayPointDataService, _locationService, _appSettingService)
            });

            _pages.Add(new MyFishTabbedPage()
            {
                BindingContext =
                  new MyFishViewModel(_navigation, _lakeService, _speciesDataService,
                      _wayPointDataService, _locationService, _appSettingService)
            });
            
            _pages.Add(new TackleBoxPage()
            {
                BindingContext =
                   new TackleBoxViewModel(_navigation, _lakeService, _speciesDataService,
                       _wayPointDataService, _locationService, _appSettingService)
            });

            _pages.Add(new LakesListProvisioningPage()
            {
                BindingContext =
                  new TackleBoxViewModel(_navigation, _lakeService, _speciesDataService,
                      _wayPointDataService, _locationService, _appSettingService)
            });

            _pages.Add(new MySpeciesProvisioningPage()
            {
                BindingContext =
                  new TackleBoxViewModel(_navigation, _lakeService, _speciesDataService,
                      _wayPointDataService, _locationService, _appSettingService)
            });



            return _pages;
        }
    }
}
