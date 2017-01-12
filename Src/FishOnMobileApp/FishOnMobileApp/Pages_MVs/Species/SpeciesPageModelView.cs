﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.Model;
using FishOn.PlatformInterfaces;
using FishOn.Services;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FishOn.ModelView
{
    public class SpeciesPageModelView : BaseModelView
    {
        private ObservableCollection<Species> _speciesList;
       
        public SpeciesPageModelView(INavigation navigation) : base(navigation) { }
        public SpeciesPageModelView(INavigation navigation, ILakeDataService lakeDataService, ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService, IFishOnCurrentLocationService locationService, IAppSettingService appSettingService):base(navigation, lakeDataService, speciesDataService, wayPointDataService, locationService, appSettingService) { }

        public async Task Initialize()
        {
            var list = (await _speciesDataService.GetSpeciesAsync()).Where(s => s.IsAvailableOnCatchList);
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