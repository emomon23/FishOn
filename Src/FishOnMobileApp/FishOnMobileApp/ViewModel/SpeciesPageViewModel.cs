using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.Model;
using FishOn.Services;
using Xamarin.Forms;

namespace FishOn.ViewModel
{
    public class SpeciesPageViewModel : BaseViewModel
    {
        private ObservableCollection<Species> _speciesList;
        private Species _selectedSpecies;

        public SpeciesPageViewModel(INavigation navigation) : base(navigation) { }
        public SpeciesPageViewModel(INavigation navigation, ILakeDataService lakeDataService, ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService):base(navigation, lakeDataService, speciesDataService, wayPointDataService) { }

        public async Task Initialize()
        {
            var list = await _speciesDataService.GetSpecies();
            var observable = new ObservableCollection<Species>(list);
            SpeciesList = observable;
        }
        
        public ICommand CancelCommand
        {
            get
            {
                return new Command(async () => await Navigate_BackToLandingPage() );
            }
        }

        public ICommand SelectSpeciesCommand
        {
            get
            {
                return new Command<Species>(async (Species s) =>
                {
                    try
                    {
                        IsBusy = true;
                        //Get current coordinates
                        //Create waypoint()
                        await _wayPointDataService.Save(new WayPoint());
                        await Navigate_BackToLandingPage();
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

        public Species SelectedSpecies
        {
            get { return _selectedSpecies; }
            set
            {
                _selectedSpecies = value;
                OnPropertyChanged();
            }
        }

    }
}
