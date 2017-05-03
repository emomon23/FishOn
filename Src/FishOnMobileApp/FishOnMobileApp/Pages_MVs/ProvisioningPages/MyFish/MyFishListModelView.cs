using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.Model.ViewModel;
using FishOn.ModelView;
using FishOn.PlatformInterfaces;
using FishOn.Repositories;
using FishOn.Services;
using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.MyFish
{
    public class MyFishListModelView : BaseModelView
    {
        private Color _viewByWayPointButonColor;
        private Color _viewBySpeciesButtonColor;

        private ObservableCollection<FishOnGroupByWayPointViewModel> _wayPointFishCaught;
        private List<Model.WayPoint> _wayPoints;
        private bool _viewByWayPoint = true;
        private bool _viewBySpecies = false;
        private bool _byWayPointButtonEnabled = false;
        private bool _bySpeciesButtonEnabled = true;

        private ObservableCollection<FishOnGroupBySpeciesViewModel> _speciesCaught;

        public MyFishListModelView(FishOnNavigationService navService, IFishOnService fishOnService):base(navService, fishOnService)
        {
            _viewByWayPointButonColor = StyleSheet.LargeToggle_SelectedBackColor;
            _viewBySpeciesButtonColor = StyleSheet.Button_BackColor;
        }


        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            _wayPoints = await _fishOnService.GetWayPointsAsync();
        
            FishCaughtByWayPoint = FishOnGroupByWayPointViewModel.MapToObservableCollection(_wayPoints);
            FishCaughtBySpecies = FishOnGroupBySpeciesViewModel.MapToObservableCollection(_wayPoints);
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
            set
            {
                _wayPointFishCaught = value;
                OnPropertyChanged();
            }
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
            set
            {
                _speciesCaught = value;
                OnPropertyChanged();
            }
        }
        
        public async Task FishTapped(Model.FishOn item)
        {
            var x = 1;
        }

        public bool ViewByWayPoint
        {
            get
            {
                return _viewByWayPoint;
            }
            set
            {
                _viewByWayPoint = value;
                OnPropertyChanged();
            }
        }

        public bool ViewBySpeices
        {
            get
            {
                return _viewBySpecies;
            }
            set
            {
                _viewBySpecies = value;
                OnPropertyChanged();
            }
        }

        #region Toggle buttons
        public Color ByWayPointButtonColor
        {
            get { return _viewByWayPointButonColor; }
            set
            {
                _viewByWayPointButonColor = value;
                OnPropertyChanged();
            }
        }

        public Color BySpeciesButtonColor
        {
            get
            {
                return _viewBySpeciesButtonColor;
            }
            set
            {
                _viewBySpeciesButtonColor = value;
                OnPropertyChanged();
            }
        }

        public bool ByWayPointButtonEnabled
        {
            get
            {
                return _byWayPointButtonEnabled;
            }
            set
            {
                _byWayPointButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool BySpeciesButtonEnabled
        {
            get { return _bySpeciesButtonEnabled; }
            set
            {
                _bySpeciesButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        public ICommand ToggleViews
        {
            get
            {
                return new Command(() =>
                {
                    ViewBySpeices = ViewByWayPoint;
                    ViewByWayPoint = !ViewByWayPoint;

                    var color = ByWayPointButtonColor;
                    ByWayPointButtonColor = BySpeciesButtonColor;
                    BySpeciesButtonColor = color;

                    BySpeciesButtonEnabled = ByWayPointButtonEnabled;
                    ByWayPointButtonEnabled = !ByWayPointButtonEnabled;
                });
            }
        }
        #endregion
    }
}
