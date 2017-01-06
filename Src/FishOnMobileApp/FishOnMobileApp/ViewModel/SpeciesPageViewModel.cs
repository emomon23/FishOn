using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.Model;
using FishOn.PlatformInterfaces;
using FishOn.Services;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FishOn.ViewModel
{
    public class SpeciesPageViewModel : BaseViewModel
    {
        private ObservableCollection<Species> _speciesList;
       
        public SpeciesPageViewModel(INavigation navigation) : base(navigation) { }
        public SpeciesPageViewModel(INavigation navigation, ILakeDataService lakeDataService, ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService, IFishOnCurrentLocationService locationService):base(navigation, lakeDataService, speciesDataService, wayPointDataService, locationService) { }

        public async Task Initialize()
        {
            var list = await _speciesDataService.GetSpeciesAsync();
            var observable = new ObservableCollection<Species>(list);
            SpeciesList = observable;
        }
        
        public ICommand CancelCommand
        {
            get
            {
                return new Command(async () => await Navigate_BackToLandingPageAsync() );
            }
        }

        public ICommand SelectSpeciesCommand
        {
            get
            {
                return new Command<Species>((Species speciesCaught) =>
                {
                    IsBusy = true;
                 
                    _locationService.GetCurrentPosition(async (Position? p, string errorMsg) =>
                    {
                        if (p.HasValue)
                        {
                            var position = p.Value;
                            await _wayPointDataService.CreateNewFishOnAsync(position.Latitude, position.Longitude,
                                speciesCaught);

                            IsBusy = false;
                            await Navigate_BackToLandingPageAsync();
                           
                        }

                    });

                });
            }
        }

        public ObservableCollection<Species> SpeciesList
        {
            get {
                return _speciesList;
            }
            private set
            {
                _speciesList = value;
                OnPropertyChanged();
            }
        }
    }
}
