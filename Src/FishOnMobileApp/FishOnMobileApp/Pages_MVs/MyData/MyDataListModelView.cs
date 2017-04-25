using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.Pages_MVs.ProvisioningPages.Lakes;
using FishOn.Pages_MVs.ProvisioningPages.MyFish;
using FishOn.Pages_MVs.ProvisioningPages.Species;
using FishOn.Pages_MVs.ProvisioningPages.TackleBox;
using FishOn.PlatformInterfaces;
using FishOn.ProvisioningPages.WayPoints;
using FishOn.Repositories;
using FishOn.Services;
using FishOn.Utils;
using Xamarin.Forms;

namespace FishOn.ModelView
{
    public class MyDataListModelView : BaseModelView
    {
        private List<Page> _pages = new List<Page>();

        public MyDataListModelView(INavigation navigation) : base(navigation) { }
        public MyDataListModelView(INavigation navigation, ILakeDataService lakeDataService, ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService, IFishOnCurrentLocationService locationService, IAppSettingService appSettingService, IFishCaughtDataService fishCaughtDataService, ISessionDataService sessionDataService) 
            :base(navigation, lakeDataService, speciesDataService, wayPointDataService, locationService, appSettingService, fishCaughtDataService, sessionDataService) { }
     
        public async Task InitializeNewPageContextAsync(string pageTitle)
        {
            //Don't iniitalize every content page on the tab
            //wait until the user wants to view that content page.
            var p = _pages.FindPage(pageTitle);

            if (p != null && p.BindingContext != null)
            {
                var pageVM = (BaseModelView) p.BindingContext;
                await pageVM.InitializeAsync();
            }
        }

        //Unable to bind the TabbedPage.Children to "{Binding ChildPages}"
        public List<Page> GetContentPages()
        {
            _pages = new List<Page>();

           
            return _pages;
        }
    }
}
