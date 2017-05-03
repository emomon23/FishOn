using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.Model;
using FishOn.PlatformInterfaces;
using FishOn.Repositories;
using FishOn.Services;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FishOn.ModelView
{
    public class SpeciesPageModelView : BaseModelView
    {
        private ObservableCollection<Species> _speciesList;
       
        public SpeciesPageModelView(FishOnNavigationService navigation, IFishOnService fishOnService) : base(navigation, fishOnService) { }
      
        public async Task Initialize()
        {
            var list = (await _fishOnService.GetSpeciesListAsync()).Where(s => s.IsAvailableOnCatchList);
            var observable = new ObservableCollection<Species>(list);
            SpeciesList = observable;
        }
        
        public ICommand CancelCommand
        {
            get
            {
                return new Command(async () => await _navigation.Navigate_BackToLandingPageAsync() );
            }
        }

        public ICommand SelectSpeciesCommand
        {
            get
            {
                return new Command<string>(async (string speciesName) =>
                {
                    IsBusy = true;
                    await _navigation.Navigate_ToVoiceToTextPage(speciesName);
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
