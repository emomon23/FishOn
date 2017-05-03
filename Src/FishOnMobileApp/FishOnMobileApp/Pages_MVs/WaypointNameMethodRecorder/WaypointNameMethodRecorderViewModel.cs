using System.Collections.Generic;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.Model.ViewModel;
using FishOn.ModelView;
using FishOn.PlatformInterfaces;
using FishOn.Services;
using FishOn.Utils;

namespace FishOn.Pages_MVs.WaypointNameMethodRecorder
{
    public class WaypointNameMethodRecorderViewModel : BaseModelView
    {
        private List<WayPoint> _wayPoints;
        private List<Lake> _lakes;
        private List<FishingMethod> _fishingMethods;

        private ISessionDataService _sessionDataService;
        private IWeatherService _weatherService;
        private IFishOnCurrentLocationService _locationService;
      
        public WaypointNameMethodRecorderViewModel(FishOnNavigationService navigation, IFishOnService fishOnService, IWeatherService weatherService, ISessionDataService sessionDataService, IFishOnCurrentLocationService locationService):base(navigation, fishOnService)
        {
            _weatherService = weatherService;
            _sessionDataService = sessionDataService;
            _locationService = locationService;
        }

        public Species SelectedSpecies { get; set; }

        public async override Task InitializeAsync()
        {
            base.InitializeAsync();

            _wayPoints = await  _fishOnService.GetWayPointsAsync();
            _lakes = await _fishOnService.GetLakesAsync();
            _fishingMethods = await _fishOnService.GetFishingMethodsAsync();

            var currentWeatherConditions = await _weatherService.GetCurrentWeatherConditions();

            this.NewFishOn = new NewFishOnViewModel()
            {
                MoonPhase = $"{currentWeatherConditions?.Moon_IlluminationPercent}%",
                Conditions = currentWeatherConditions?.WeatherSummary,
                WaterTemp = _sessionDataService.CurrentWaterTemp,
                LakeName = _sessionDataService.CurrentLake?.LakeName,
                SpeciesId = SelectedSpecies.SpeciesId
            };

            var lastFishCaught = await _fishOnService.GetMostRecentFishCaughtAsync(this.SelectedSpecies.SpeciesId, 30);
            if (lastFishCaught != null)
            {
                NewFishOn.WayPointName = lastFishCaught.WayPointName;
                NewFishOn.Latitude = lastFishCaught.Latitude;
                NewFishOn.Longitude = lastFishCaught.Longitude;
                if (lastFishCaught.SpeciesId == SelectedSpecies.SpeciesId)
                {
                    NewFishOn.FishingMethod = lastFishCaught.FishingMethod;
                }
            }

            if (!NewFishOn.Latitude.HasValue)
            {
                GetWaypointCoordinates();
            }
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

        public async Task SaveNewFishCaught()
        {
            if (NewFishOn.Conditions.IsNotNullOrEmpty())
            {
                var conditions = await _weatherService.GetCurrentWeatherConditions();
                if (conditions != null)
                {
                    NewFishOn.Conditions = conditions.WeatherSummary;
                    if (NewFishOn.MoonPhase.IsNotNullOrEmpty())
                    {
                        NewFishOn.MoonPhase = conditions.Moon_IlluminationPercent + "%";
                    }
                }
            }
            await _fishOnService.SaveFishCaughtAsync(NewFishOn);
            _navigation.Navigate_BackToLandingPageAsync("Waypoint Saved");
        }

        private void GetWaypointCoordinates()
        {
            _locationService.GetCurrentPosition(async (position, message) =>
            {
                if (position.HasValue)
                {
                    NewFishOn.Latitude = position.Value.Latitude;
                    NewFishOn.Longitude = position.Value.Longitude;
                }
            });
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

        public NewFishOnViewModel NewFishOn { get; set; }

        public IList<WayPoint> WayPointList
        {
            get
            {
               return _wayPoints;
            }
        }

        public IList<FishingMethod> FishingMethodsList
        {
            get
            {
                return _fishingMethods;
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

            await _navigation.Navigate_BackToLandingPageAsync();
        }

      

    }
}
