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
using FishOn.Utils;
using Plugin.Media.Abstractions;
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
                MoonPhase = $"{_sessionDataService.CurrentWeatherCondition?.Moon_IlluminationPercent}% FM",
                Conditions = _sessionDataService.CurrentWeatherCondition?.WeatherSummary,
                WaterTemp = _sessionDataService.CurrentWaterTemp,
                LakeName = _sessionDataService.CurrentLake?.LakeName
            };
        }

        public string SelectedSpecies { get; set; }

        public async override Task InitializeAsync()
        {
            base.InitializeAsync();
            _wayPoints = await _wayPointDataService.GetWayPointsAsync();
            _lakes = await _lakeService.GetLakesAsync();
        }

        public async Task<CameraResult> TakePicture()
        {
            FishOnCamera camera = new FishOnCamera();
            var cameraResult = await camera.TakePictureAsync();
          
            if (NewFishOn.Image1FileName.IsNullOrEmpty())
            {
                NewFishOn.Image1FileName = cameraResult.FullPath;
                OnPropertyChanged("FishOn.Image1FileName");
            }
            else
            {
                if (NewFishOn.Image2FileName.IsNotNullOrEmpty())
                {
                    FishOnCamera.DeleteImages(NewFishOn.Image1FileName);
                    NewFishOn.Image1FileName = NewFishOn.Image2FileName;
                    OnPropertyChanged("FishOn.Image1FileName");
                }

                NewFishOn.Image2FileName = cameraResult.FullPath;
               
                OnPropertyChanged("FishOn.Image2FileName");
            }

            return cameraResult;
        }

        public bool IsLakeNameInWayPointName
        {
            get
            {
                var tempLower = NewFishOn.WayPointName.ToLower().Split(' ');

                if (tempLower.Length > 2)
                {
                    return tempLower[0] == "lake" || tempLower[1] == "lake";
                }

                return false;
            }
        }

        public string ParseOutLakeNameFromWayPointName()
        {
            var tempLower = NewFishOn.WayPointName.ToLower().Split(' ');
            var result = "";

            if (tempLower.Length > 2)
            {
                int startingIndex = 0;

                if (tempLower[0] == "lake")
                {
                    result = NewFishOn.WayPointName.Split(' ')[1].Capitalize();
                    startingIndex = 6 + result.Length;

                }
                else if (tempLower[1] == "lake")
                {
                    result = NewFishOn.WayPointName.Split(' ')[0].Capitalize();
                    startingIndex = 6 + result.Length;
                }

                if (startingIndex > 0)
                {
                    WayPointNameWithLakeParseOut = NewFishOn.WayPointName.Substring(startingIndex).Capitalize();
                }
            }

            return result;
        }

        public string WayPointNameWithLakeParseOut { get; private set; }

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
            FishOnCamera.DeleteImages(NewFishOn.Image1FileName, NewFishOn.Image2FileName);

            await Navigate_BackToLandingPageAsync("");
        }

    }
}
