using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.Model;
using FishOn.Model.ViewModel;
using FishOn.PlatformInterfaces;
using FishOn.Services;
using FishOn.Utils;
using FishOn.ModelView;
using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.MyFish
{
    public class MyFishModelView : BaseModelView
    {
        private List<Page> _pages = new List<Page>();
        private List<WayPoint> _wayPoints;
     
        private ObservableCollection<FishOnGroupBySpeciesViewModel> _speciesCaught = null;
        private ObservableCollection<FishOnGroupByWayPointViewModel> _wayPointFishCaught = null;

        public MyFishModelView(INavigation navigation) : base(navigation) { }
        public MyFishModelView(INavigation navigation, ILakeDataService lakeDataService, ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService, IFishOnCurrentLocationService locationService, IAppSettingService appSettingService):base(navigation, lakeDataService, speciesDataService, wayPointDataService, locationService, appSettingService) { }

        public string Title
        {
            get { return "My Fish"; }
        }

        public async Task<List<Page>> GetContentPagesAsync()
        {
            if (_pages.Count == 0)
            {
                _wayPoints = await _wayPointDataService.GetWayPointsAsync();

                _pages.Add(new MyFishByWayPoints() {BindingContext = this});
                _pages.Add(new MyFishBySpecies() {BindingContext = this});
            }

            return _pages;
        }

        public List<WayPoint> WayPointFishList
        {
            get
            {
                return _wayPoints;
            }
        }

        public Model.FishOn FishCaught { get; private set; }
        public async Task EditFishOnAsync(Model.FishOn fish)
        {
            FishCaught = fish;
            await Navigate_ToFishOnProvisioningPage();
        }


        public ObservableCollection<FishOnGroupBySpeciesViewModel> FishCaughtBySpecies
        {
            get
            {
                if (_speciesCaught == null)
                {
                    _speciesCaught = FishOnGroupBySpeciesViewModel.MapToObservableCollection(_wayPoints);
                }
                return _speciesCaught;
            }
        }

        public ObservableCollection<FishOnGroupByWayPointViewModel> FishCaughtByWayPoint
        {
            get
            {
                if (_wayPointFishCaught == null)
                {
                    _wayPointFishCaught = FishOnGroupByWayPointViewModel.MapToObservableCollection(_wayPoints);
                }

                return _wayPointFishCaught;
            }
        }

        #region WeatherAccordianNode

        protected int _weatherExpandingHeight = 0;
        public int WeatherExpandedHeight
        {
            get
            {
                return _weatherExpandingHeight;
            }
            private set
            {
                _weatherExpandingHeight = value;
                OnPropertyChanged();
            }
        }
        
        public ICommand ExpandHideWeatherData
        {
            get
            {
                return new Command(() =>
                {
                    if (WeatherExpanderButtonText == "+")
                    {
                        WeatherExpandedHeight = 200;
                        WeatherExpanderButtonText = "-";
                     
                    }
                    else
                    {
                        WeatherExpandedHeight = 0;
                        WeatherExpanderButtonText = "+";
                    
                    }
                });
            }
        }

        protected string _weatherExpanderButtonText = "+";
        public string WeatherExpanderButtonText
        {
            get
            {
                return _weatherExpanderButtonText;
            }
            private set
            {
                _weatherExpanderButtonText = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region LureAccordianNode 

        protected int _lureExpandingHeight = 0;
        public int LureExpandedHeight
        {
            get
            {
                return _lureExpandingHeight;
            }
            private set
            {
                _lureExpandingHeight = value;
                OnPropertyChanged();
            }
        }

        public ICommand ExpandHideLureData
        {
            get
            {
                return new Command(() =>
                {
                    if (LureExpanderButtonText == "+")
                    {
                        LureExpandedHeight = 200;
                        LureExpanderButtonText = "-";
                        IsLureExpanded = true;
                    }
                    else
                    {
                        LureExpandedHeight = 0;
                        LureExpanderButtonText = "+";
                        IsLureExpanded = false;
                    }
                });
            }
        }

        protected string _lureExpanderButtonText = "+";
        public string LureExpanderButtonText
        {
            get
            {
                return _lureExpanderButtonText;
            }
            private set
            {
                _lureExpanderButtonText = value;
                OnPropertyChanged();
            }
        }

        private bool _isLureExpanded = false;
        public bool IsLureExpanded
        {
            get
            {
                return _isLureExpanded;
            }
            private set
            {
                _isLureExpanded = value;
                OnPropertyChanged();
            }
        }


        #endregion
    }

}
