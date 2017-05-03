using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.PlatformInterfaces;
using FishOn.Services;
using FishOn.Utils;
using Xamarin.Forms;

namespace FishOn.ModelView
{
    public class MainPageModelView : BaseModelView
    {
        private bool _initialzeCalled = false;
        private ISessionDataService _sessionDataService;
        private IFishOnCurrentLocationService _locationService;

        public MainPageModelView(FishOnNavigationService navigation, IFishOnService fishOnService, ISessionDataService sessionDataService, IFishOnCurrentLocationService locationService) : base(navigation, fishOnService)
        {
            _sessionDataService = sessionDataService;
            _locationService = locationService;
        }
      
        public override async Task InitializeAsync()
        {
            if (!_initialzeCalled)
            {
                //We don't want to execute this logic everytime the view appears
                _initialzeCalled = true;

                //If not lakes exist, prompt the user for this one time
                var lakes = await _fishOnService.GetLakesAsync();
                if (!lakes.Any())
                {
                    var modalDialogViewModal = new SimpleInputModalModelView(_navigation,
                        "Tell us some of the lakes you like to fish (eg. Tonka,Big Lake)?");

                    await modalDialogViewModal.DisplayModalAsync(
                        async (bool cancelClicked, string commaSeperatedListOfLakeNames, bool dontMatter) =>
                        {
                            if (!cancelClicked && commaSeperatedListOfLakeNames.IsNotNullOrEmpty())
                            {
                                await _fishOnService.CreateNewLakesAsync(commaSeperatedListOfLakeNames.Split(','));
                            }
                        });
                }
                else
                {
                    //no await here, just get them cached in the background.
                    _fishOnService.GetWayPointsAsync();
                }

                if (_sessionDataService.CurrentWeatherCondition == null)
                {
                    IWeatherService weatherService = new WeatherService(_sessionDataService);
                    _locationService.GetCurrentPosition(async (position, message) =>
                    {
                        if (position.HasValue)
                        {
                            var conditions =  await weatherService.GetWeatherConditionsAsync(position.Value.Latitude, position.Value.Longitude);
                            _sessionDataService.CurrentWeatherCondition = conditions;
                            _sessionDataService.InitialPosition = position.Value;
                        }
                    });
                }
            }

            _fishOnService.GetAppSettingsAsync();
        }

        public ICommand FishOnCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await _navigation.Navigate_ToSpeciesListAsync();
                });
            }
        }

        public ICommand SetLakeWaterTemp
        {
            get
            {
                return new Command(async () =>
                {
                    await _navigation.Navigate_ToLakeListAsync();
                });
            }
        }

        public ICommand MyDataCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await _navigation.Navigate_ToMyDataButtonsListAsync();
                });
            }
        }

        public ICommand FindMySpotCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await _navigation.Naviage_ToLakeMapAsync();
                });
            }
        }
    }
    
}