using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.Model;
using FishOn.PlatformInterfaces;
using FishOn.ProvisioningPages.WayPoints;
using FishOn.Repositories;
using FishOn.Services;
using FishOnMobileApp;
using Xamarin.Forms;

namespace FishOn.ModelView
{
    public class WayPointProvisioningModelView : BaseModelView
    {
        private ObservableCollection<WayPoint> _speciesList;
        private List<Lake> _lakeList;

        public WayPointProvisioningModelView(FishOnNavigationService navigationService, IFishOnService fishOnService):base(navigationService, fishOnService) { }

        public override async Task InitializeAsync()
        {
            await GetWayPointList();
           _lakeList = await _fishOnService.GetLakesAsync();
        }

        private async Task GetWayPointList()
        {
            var wayPoints = await _fishOnService.GetWayPointsAsync();
            WayPointList = new ObservableCollection<WayPoint>(wayPoints);
        }

        public List<Lake> LakeList
        {
            get { return _lakeList; }
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

        public int SelectedLakeIndex
        {
            get
            {
                int result = 0;
                for (int i = 0; i < _lakeList.Count; i++)
                {
                    if (_lakeList[i].LakeId == WayPoint.LakeId)
                    {
                        result = i;
                        break;
                    }
                }

                return result;
            }
            set
            {
                WayPoint.LakeId = _lakeList[value].LakeId;
                WayPoint.Lake = _lakeList[value];
            }  
        }

        public ICommand SaveWayPointCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var lake = (await _fishOnService.CreateNewLakesAsync(LakeName))[0];
                    WayPoint.LakeId = lake.LakeId;
                    await _fishOnService.SaveWayPointProvisioningAsync(WayPoint);
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
                        await _fishOnService.DeleteWayPointAsync(WayPoint);
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
                    var lake = (await _fishOnService.GetLakesAsync()).FirstOrDefault(l => l.LakeId == wayPoint.LakeId);
                    WayPoint = wayPoint;
                    LakeName = lake?.LakeName;
                    await _navigation.Naviage_ToWayPointProvisioningDetailPage();
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
                    await _navigation.Naviage_ToWayPointProvisioningDetailPage();
                });
            }
        }

       


    }
  
}
