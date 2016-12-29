using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.Model;
using FishOn.Services;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;

namespace FishOn.ViewModel
{
    public class SpeciesPageViewModel : BaseViewModel
    {
        private ObservableCollection<Species> _speciesList;
       
        public SpeciesPageViewModel(INavigation navigation) : base(navigation) { }
        public SpeciesPageViewModel(INavigation navigation, ILakeDataService lakeDataService, ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService):base(navigation, lakeDataService, speciesDataService, wayPointDataService) { }

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
                return new Command<Species>(async (Species speciesCaught) =>
                {
                    try
                    {
                        IsBusy = true;
                        var position = await CrossGeolocator.Current.GetPositionAsync(10000);
                        await _wayPointDataService.CreateNewFishOnAsync(position.Latitude, position.Longitude, speciesCaught);
                        await Navigate_BackToLandingPageAsync();
                    }
                    catch (Exception exp)
                    {
                        var needToHandleThis = exp.Message;
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                });
            }
        }

        public ObservableCollection<Species> SpeciesList
        {
            get { return _speciesList; }
            private set
            {
                _speciesList = value;
                OnPropertyChanged();
            }
        }
    }
}
