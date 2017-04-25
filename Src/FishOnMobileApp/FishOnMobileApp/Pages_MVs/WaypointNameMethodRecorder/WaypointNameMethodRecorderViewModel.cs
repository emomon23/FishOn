using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.Model.ViewModel;
using FishOn.ModelView;
using FishOn.Pages_MVs.ProvisioningPages;
using FishOn.PlatformInterfaces;
using FishOn.Repositories;
using FishOn.Services;
using Xamarin.Forms;

namespace FishOn.Pages_MVs.WaypointNameMethodRecorder
{
    public class WaypointNameMethodRecorderViewModel : BaseModelView
    {
        private List<WayPoint> _wayPoints;
        private List<Lake> _lakes;

        public WaypointNameMethodRecorderViewModel(INavigation navigation, ILakeDataService lakeDataService,
            ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService,
            IFishOnCurrentLocationService fishOnCurrentLocationService, IAppSettingService appSettingService,
            IFishCaughtDataService fishCaughtDataService, ISessionDataService sessionDataService)
            : base(
                navigation, lakeDataService, speciesDataService, wayPointDataService, fishOnCurrentLocationService,
                appSettingService, fishCaughtDataService, sessionDataService)
        {
            this.NewFishOn = new CaugtFish()
            {
                MoonPhase = _sessionDataService.CurrentWeatherCondition?.MoonSummary,
                Conditions = _sessionDataService.CurrentWeatherCondition?.WeatherSummary,
                WaterTemp = _sessionDataService.CurrentWaterTemp,
                LakeName = _sessionDataService.CurrentLake?.LakeName
            };

        }

        public async override Task InitializeAsync()
        {
            base.InitializeAsync();
            _wayPoints = await _wayPointDataService.GetWayPointsAsync();
            _lakes = await _lakeService.GetLakesAsync();
        }

        public CaugtFish NewFishOn { get; set; }

        public IList<WayPoint> WayPointList
        {
            get
            {
               return _wayPoints;
            }
        }

        public IList<Lake> AvailableLakes
        {
            get
            {
                return _lakes;
            }
        }

        public async Task Cancel()
        {
            await Navigate_BackToLandingPageAsync("");
        }

    }
}
