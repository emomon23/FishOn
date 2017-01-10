using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.PlatformInterfaces;
using FishOn.Services;
using FishOn.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FishOn.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        private bool _initialzeCalled = false;
        public MainPageViewModel(INavigation navigation) : base(navigation){}
        public MainPageViewModel(INavigation navigation, ILakeDataService lakeDataService, ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService, IFishOnCurrentLocationService locationService, IAppSettingService appSettingService):base(navigation, lakeDataService, speciesDataService, wayPointDataService, locationService, appSettingService) { }

        public async Task Initialize()
        {
            if (!_initialzeCalled)
            {
                //We don't want to execute this logic everytime the view appears
                _initialzeCalled = true;
                var lakes = await _lakeService.GetLakesAsync();
                if (lakes.Count == 0)
                {
                    var modalDialogViewModal = new SimpleInputModalViewModel(_navigation,
                        "Tell us some of the lakes you like to fish (eg. Tonka,Big Lake)?");

                    await modalDialogViewModal.DisplayModalAsync(async (bool cancelClicked, string commaSeperatedListOfLakeNames) =>
                        {
                            if (!cancelClicked && commaSeperatedListOfLakeNames.IsNotNullOrEmpty())
                            {
                                await _lakeService.CreateNewLakesAsync(commaSeperatedListOfLakeNames.Split(','));
                            }
                        });
                }
            }

            await _appSettingService.Initialize();
        }

        public ICommand FishOnCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Navigate_ToSpeciesListAsync();
                });
            }
        }

        public ICommand SetLakeWaterTemp
        {
            get
            {
                return new Command(async () =>
                {
                    await Navigate_ToLakeListAsync();
                });
            }
        }

        public ICommand MyDataCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Navigate_ToMyDataButtonsListAsync();
                });
            }
        }

        public ICommand FindMySpotCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Naviage_ToLakeMapAsync();
                });
            }
        }
    }
    
}