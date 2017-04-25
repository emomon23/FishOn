using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.ModelView;
using FishOn.Pages_MVs.ProvisioningPages.Lakes;
using FishOn.Pages_MVs.ProvisioningPages.MyFish;
using FishOn.Pages_MVs.ProvisioningPages.Species;
using FishOn.Pages_MVs.ProvisioningPages.TackleBox;
using FishOn.PlatformInterfaces;
using FishOn.ProvisioningPages.WayPoints;
using FishOn.Repositories;
using FishOn.Services;
using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages
{
    public enum provisioningPageEnumeration
    {
        WayPoints,
        MyFish,
        Lakes,
        Tackle,
        Species
    };

    public class ProvisioningPageDefinitions
    {
        private List<ProvisioningPageDefinition> _definitions;

        public ProvisioningPageDefinitions()
        {
            _definitions = new List<ProvisioningPageDefinition>()
            {
                new ProvisioningPageDefinition(provisioningPageEnumeration.WayPoints),
                new ProvisioningPageDefinition(provisioningPageEnumeration.MyFish),
                new ProvisioningPageDefinition(provisioningPageEnumeration.Lakes),
                new ProvisioningPageDefinition(provisioningPageEnumeration.Tackle),
                new ProvisioningPageDefinition(provisioningPageEnumeration.Species)
            };
        }

        public List<ProvisioningPageDefinition> GetList()
        {
            return _definitions;
        }

        public ProvisioningPageDefinition GetDefinition(string pageTitle)
        {
            var definition = _definitions.FirstOrDefault(d => d.PageTitle == pageTitle);
            if (definition == null)
            {
                throw new Exception($"Unable to find page title '{pageTitle}' in list page defintiions");
            }

            return definition;
        }
    }

    public class ProvisioningPageDefinition
    {
        private provisioningPageEnumeration _pageEnum;

        public ProvisioningPageDefinition(provisioningPageEnumeration provisioningPage)
        {
            _pageEnum = provisioningPage;
            PageTitle = _pageEnum.ToString().Replace("WayPoints", "Way Points").Replace("MyFish", "My Fish");
        }

        public string PageTitle { get; private set; }

        public async Task<ContentPage> CreatePage(INavigation navigation, ILakeDataService lakeDataService, ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService, IFishOnCurrentLocationService locationService, IAppSettingService appSettingService, IFishCaughtDataService fishCaughtDataService, ISessionDataService sessionDataService)
        {
            ContentPage result = null;
            BaseModelView vm = null;

            switch (_pageEnum)
            {
                case provisioningPageEnumeration.WayPoints:
                    vm = new WayPointProvisioningModelView(navigation, lakeDataService, speciesDataService, wayPointDataService, locationService, appSettingService, fishCaughtDataService, sessionDataService);
                    await vm.InitializeAsync();
                    result = new WPProvisoningList((WayPointProvisioningModelView)vm);
                    break;
                case provisioningPageEnumeration.Lakes:
                    vm = new LakeListProvisioningModelView(navigation, lakeDataService, speciesDataService,
                        wayPointDataService, locationService, appSettingService, fishCaughtDataService,
                        sessionDataService);
                    await vm.InitializeAsync();
                    result = new LakesListProvisioningPage((LakeListProvisioningModelView)vm);
                    break;
                case provisioningPageEnumeration.MyFish:
                    vm = new MyFishListModelView(navigation, lakeDataService, speciesDataService, wayPointDataService, locationService, appSettingService, fishCaughtDataService, sessionDataService);
                    await vm.InitializeAsync();
                    result = new MyFishList((MyFishListModelView)vm);
                    break;
             
                case provisioningPageEnumeration.Tackle:
                    vm = new TackleBoxModelView(navigation, lakeDataService, speciesDataService, wayPointDataService, locationService, appSettingService, fishCaughtDataService, sessionDataService);
                    await vm.InitializeAsync();
                    result = new TackleBoxPage((TackleBoxModelView)vm);
                    break;
                case provisioningPageEnumeration.Species:
                    vm = new MySpeciestProvisioningViewModel(navigation, lakeDataService, speciesDataService, wayPointDataService, locationService, appSettingService, fishCaughtDataService, sessionDataService);
                    await vm.InitializeAsync();
                    result = new MySpeciesProvisioningPage((MySpeciestProvisioningViewModel)vm);
                    break;
            }

            return result;
        }
    }
}
