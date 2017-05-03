using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.PlatformInterfaces;
using FishOn.Repositories;
using FishOn.Services;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FishOn.ModelView
{
    public class LakeMapPageModelView : BaseModelView
    {
        private List<WayPoint> _wayPoints;
        private Map _lakeMap;
        private IFishOnCurrentLocationService _locationService;
        private ObservableCollection<Species> _speciesList;
        private List<Species> _displaySpecies;

        public LakeMapPageModelView(FishOnNavigationService navigation, IFishOnService fishOnService, IFishOnCurrentLocationService locationService) : base(navigation, fishOnService)
        {
            _locationService = locationService;
        }
        
        public Map LakeMapControl {
            get { return _lakeMap; }
            set { _lakeMap = value; }
        }

        public async Task InitializeAsync()
        {
            await GetSpecies();
            _wayPoints = await _fishOnService.GetWayPointsAsync();
            
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
            get { return !_fishOnService.HasMapFilterEverBeenUsed; }
        }

        public async Task SpeciesFilterSwitch(Species species)
        {
            _fishOnService.HasMapFilterEverBeenUsed = true;
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
            var species = await _fishOnService.GetSpeciesListAsync();
            _displaySpecies = species.Where(s => s.DisplaySpeciesOnLakeMap).ToList();
            SpeciesList = new ObservableCollection<Species>(species);
        }
    }
}
