using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.PlatformInterfaces;
using FishOn.Services;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FishOn.ViewModel
{
    public class LakeMapPageViewModel : BaseViewModel
    {
        private List<WayPoint> _wayPoints;
        private Map _lakeMap;
        private ObservableCollection<Species> _speciesList;
        private List<Species> _displaySpecies;

        public LakeMapPageViewModel(INavigation navigation, Map lakeMap) : base(navigation)
        {
            _lakeMap = lakeMap;
        }

        public LakeMapPageViewModel(INavigation navigation, Map lakeMap, ILakeDataService lakeDataService,
            ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService, IFishOnCurrentLocationService locationService, IAppSettingService appSettingService)
            : base(navigation, lakeDataService, speciesDataService, wayPointDataService, locationService, appSettingService)
        {
            _lakeMap = lakeMap;
        }

        public async Task InitializeAsync()
        {
            await GetSpecies();
            _wayPoints = await _wayPointDataService.GetWayPointsAsync();
            
            DisplayPinList();

            IsBusy = true;

            _locationService.GetCurrentPosition(async (Position? p, string errorMsg) =>
            {
                if (p.HasValue)
                {
                    var position = p.Value;
                    double miles = .5;
                    _lakeMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(miles)));
                    IsBusy = false;
                }
            });

        }

        public ObservableCollection<Species> SpeciesList
        {
            get
            {
                return _speciesList;
            }
            set
            {
                _speciesList = value;
                OnPropertyChanged();
            }
        }

        public bool IsFadeVisible
        {
            get { return !_appSettingService.MapFilterHasBeenUsed; }
        }

        public async Task SpeciesFilterSwitch(Species species)
        {
            _appSettingService.MapFilterHasBeenUsed = true;
            DisplayPinList();
        }

        private void DisplayPinList()
        {
            _lakeMap.Pins.Clear();

           foreach (var wayPoint in _wayPoints)
            {
                if (Does_WayPoint_HaveSpecies_ToDisplay_PinFor(wayPoint))
                {
                    var position = new Position(wayPoint.Latitude, wayPoint.Longitude);
                    var pin = new Pin
                    {
                        Label = wayPoint.WayPointType == WayPoint.WayPointTypeEnumeration.FishOn ? "F" : "B",
                        Position = position,
                        Type =
                            wayPoint.WayPointType == WayPoint.WayPointTypeEnumeration.FishOn
                                ? PinType.SavedPin
                                : PinType.Place
                    };

                    _lakeMap.Pins.Add(pin);
                }
            }
        }

        private bool Does_WayPoint_HaveSpecies_ToDisplay_PinFor(WayPoint wp)
        {
            var speciesIds = wp.Species.Select(s => s.SpeciesId).ToArray();
            var result = _displaySpecies.Any(s => speciesIds.Contains(s.SpeciesId) && s.DisplaySpeciesOnLakeMap);

            return result;
        }

        private async Task GetSpecies()
        {
            var species = await _speciesDataService.GetSpeciesAsync();
            _displaySpecies = species.Where(s => s.DisplaySpeciesOnLakeMap).ToList();
            SpeciesList = new ObservableCollection<Species>(species);
        }
    }
}
