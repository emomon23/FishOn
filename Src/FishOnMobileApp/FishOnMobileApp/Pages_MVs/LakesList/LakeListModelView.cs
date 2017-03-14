using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.PlatformInterfaces;
using FishOn.Services;
using FishOn.Utils;
using Xamarin.Forms;

namespace FishOn.ModelView
{
    public class LakeListModelView : BaseModelView
    {
        private Model.Lake _selectedLake;
        private string _newLakeNames;
        private int _waterTemp;

        ObservableCollection<Lake> _lakes;

        public LakeListModelView(INavigation navigation) : base(navigation) { }
        public LakeListModelView(INavigation navigation, ILakeDataService lakeDataService, ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService, IFishOnCurrentLocationService locationService, IAppSettingService appSettingService, IFishCaughtDataService fishCaughtDataService):base(navigation, lakeDataService, speciesDataService, wayPointDataService, locationService, appSettingService, fishCaughtDataService) { }
        
        public async override Task InitializeAsync()
        {
            var list = await _lakeService.GetLakesAsync();
            var observable = new ObservableCollection<Lake>(list);
            LakesList = observable;

            if (SessionData.CurrentLakeId > 0)
            {
                SelectedLake = _lakes.SingleOrDefault(l => l.LakeId == SessionData.CurrentLakeId);
            }

            WaterTemp = SessionData.CurrentWaterTemp;
        }

        public ObservableCollection<Lake> LakesList
        {
            get
            {
                return _lakes;
            }
            private set
            {
                _lakes = value;
                OnPropertyChanged();
            }
        }

        public string NewLakeNamesSeperatedByComma
        {
            get
            {
                return _newLakeNames;
            }
            set
            {
                _newLakeNames = value;
                OnPropertyChanged();
            }
        }

        public Lake SelectedLake
        {
            get
            {
                return _selectedLake;
            }
            set
            {
                _selectedLake = value;
                OnPropertyChanged();
            }
        }

        public int WaterTemp
        {
            get
            {
                return _waterTemp;
            }
            set
            {
                _waterTemp = value;
                OnPropertyChanged();
            }
        }

        public Command AddNewLakesCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (NewLakeNamesSeperatedByComma.IsNotNullOrEmpty())
                    {
                        var lakes = await _lakeService.CreateNewLakesAsync(NewLakeNamesSeperatedByComma.Split(','));
                        LakesList = new ObservableCollection<Lake>(lakes);
                        NewLakeNamesSeperatedByComma = "";
                    }
                    else
                    {
                        //alert, you must provide some lakes!
                    }
                });
            }
        }

        public Command LakeSelectedCommand
        {
            get
            {
                return new Command<string>((string selectedLake) =>
                {
                    SelectedLake = _lakes.FirstOrDefault(l => l.LakeName == selectedLake);
                });
            }
        }

        public Command SetSessionDataCommand
        {
            get
            {
                return new Command(() =>
                {
                    SessionData.RestartSession(SelectedLake?.LakeId, _waterTemp);
                    Navigate_BackToLandingPageAsync();
                });
            }
        }
    }
}
