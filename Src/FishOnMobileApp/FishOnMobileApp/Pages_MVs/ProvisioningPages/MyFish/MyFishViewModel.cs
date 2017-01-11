using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.Model.ViewModel;
using FishOn.PlatformInterfaces;
using FishOn.Services;
using FishOn.Utils;
using FishOn.ViewModel;
using Xamarin.Forms;

namespace FishOn.Pages_VMs.ProvisioningPages.MyFish
{
    public class MyFishViewModel : BaseViewModel
    {
        private List<Page> _pages = new List<Page>();
        private List<WayPoint> _wayPoints;
        private List<SpeciesCaughtDTOModel> _speciesCaught = null;

        public MyFishViewModel(INavigation navigation) : base(navigation) { }
        public MyFishViewModel(INavigation navigation, ILakeDataService lakeDataService, ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService, IFishOnCurrentLocationService locationService, IAppSettingService appSettingService):base(navigation, lakeDataService, speciesDataService, wayPointDataService, locationService, appSettingService) { }

        public string Title
        {
            get { return "My Fish"; }
        }

        public async Task<List<Page>> GetContentPagesAsync()
        {
            if (_pages.Count == 0)
            {
                _wayPoints = await _wayPointDataService.GetWayPointsAsync();

                _pages.Add(new MyFishByWayPoints() {BindingContext = this});
                _pages.Add(new MyFishBySpecies() {BindingContext = this});
            }

            return _pages;
        }

        public List<WayPoint> WayPointFishList
        {
            get
            {
                return _wayPoints;
            }
        }

        public List<SpeciesCaughtDTOModel> FishCaughtBySpecies
        {
            get
            {
                if (_speciesCaught == null)
                {
                    _speciesCaught = new List<SpeciesCaughtDTOModel>();
                    foreach (var wp in _wayPoints)
                    {
                        foreach (var fc in wp.FishCaught)
                        {
                            _speciesCaught.AddSpeciesCaught(fc);
                        }
                    }
                }

                return _speciesCaught;
            }
        }

    }

}
