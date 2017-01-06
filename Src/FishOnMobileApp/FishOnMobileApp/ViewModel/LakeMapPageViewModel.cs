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
        private Map _lakeMap;

        public LakeMapPageViewModel(INavigation navigation, Map lakeMap) : base(navigation)
        {
            _lakeMap = lakeMap;
        }

        public LakeMapPageViewModel(INavigation navigation, Map lakeMap, ILakeDataService lakeDataService,
            ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService, IFishOnCurrentLocationService locationService)
            : base(navigation, lakeDataService, speciesDataService, wayPointDataService, locationService)
        {
            _lakeMap = lakeMap;
        }

        public async Task InitializeAsync()
        {
            var wayPoints = await _wayPointDataService.GetWayPointsAsync();
            foreach (var wayPoint in wayPoints)
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
    }
}
