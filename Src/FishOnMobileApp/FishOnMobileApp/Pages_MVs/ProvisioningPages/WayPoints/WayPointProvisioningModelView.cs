using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.Model;
using FishOn.PlatformInterfaces;
using FishOn.ProvisioningPages.WayPoints;
using FishOn.Services;
using FishOnMobileApp;
using Xamarin.Forms;

namespace FishOn.ModelView
{
    public class WayPointProvisioningModelView : BaseModelView
    {
        private ObservableCollection<WayPoint> _speciesList;
        
        public WayPointProvisioningModelView(INavigation navigation, ILakeDataService lakeDataService,
            ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService, IFishOnCurrentLocationService locationService, IAppSettingService appSettingService)
            : base(navigation, lakeDataService, speciesDataService, wayPointDataService, locationService, appSettingService){}

        public override async Task InitializeAsync()
        {
            await GetWayPointList();
        }

        private async Task GetWayPointList()
        {
            var wayPoints = await _wayPointDataService.GetWayPointsAsync();
            WayPointList = new ObservableCollection<WayPoint>(wayPoints);
        }

        public ObservableCollection<WayPoint> WayPointList
        {
            get { return _speciesList; }
            private set
            {
                _speciesList = value;
                OnPropertyChanged();
            }
        }

        public WayPoint WayPoint { get; private set; }
        public string LakeName { get; set; }
        
        public int SelectedWayPointTypeIndex
        {
            get { return WayPoint.WayPointType == WayPoint.WayPointTypeEnumeration.BoatLaunch ? 1 : 0; }
            set
            {
                WayPoint.WayPointType = value == 0
                    ? WayPoint.WayPointTypeEnumeration.FishOn
                    : WayPoint.WayPointTypeEnumeration.BoatLaunch;
            }
        }
        public ICommand SaveWayPointCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var lake = await _lakeService.GetOrCreateLakeAsync(LakeName);
                    WayPoint.LakeId = lake.LakeId;
                    await _wayPointDataService.SaveWayPointProvisioningAsync(WayPoint);
                    await GetWayPointList();
                    await _navigation.PopAsync();
                });
            }
        }

        public ICommand DeleteWayPointCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var answer = await App.Current.MainPage.DisplayAlert("Are you sure?",
                        $"Delete waypoint {WayPoint.Name}?", "Yes", "No");

                    if (answer)
                    {
                        await _wayPointDataService.DeleteWayPointAsync(WayPoint);
                        await GetWayPointList();
                        await _navigation.PopAsync();
                    }
                });
            }
        }

        public ICommand CancelWayPointAddEditCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await _navigation.PopAsync();
                });
            }
        }
        
        public ICommand EditWayPoint
        {
            get
            {
                return new Command<WayPoint>(async (WayPoint wayPoint) =>
                {
                    var lake = await _lakeService.GetLakeAsync(wayPoint.LakeId);
                    WayPoint = wayPoint;
                    LakeName = lake?.LakeName;
                    await Naviage_ToWayPointProvisioningDetailPage();
                });
            }
        }

        public ICommand AddWayPoint
        {
            get
            {
                return new Command(async () =>
                {
                    WayPoint = new WayPoint() {Name = ""};
                    LakeName = "";
                    await Naviage_ToWayPointProvisioningDetailPage();
                });
            }
        }

       


    }
  
}
