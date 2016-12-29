using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FishOn.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel(INavigation navigation) : base(navigation){}
        public MainPageViewModel(INavigation navigation, ILakeDataService lakeDataService, ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService):base(navigation, lakeDataService, speciesDataService, wayPointDataService) { }

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