using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Model;
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
            ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService)
            : base(navigation, lakeDataService, speciesDataService, wayPointDataService)
        {
            _lakeMap = lakeMap;
        }

        public async Task InitializeAsync()
        {
            var wayPoints =await _wayPointDataService.GetWayPointsAsync();
            foreach (var wayPoint in wayPoints)
            {
                var position = new Position(wayPoint.Latitude, wayPoint.Longitude);
                var pin = new Pin
                {
                    Label = wayPoint.WayPointType == WayPoint.WayPointTypeEnumeration.FishOn ? "F" : "B",
                    Position = position,
                    Type = wayPoint.WayPointType == WayPoint.WayPointTypeEnumeration.FishOn ? PinType.SavedPin : PinType.Place
                };

                _lakeMap.Pins.Add(pin);
            }
        }

    }
}
