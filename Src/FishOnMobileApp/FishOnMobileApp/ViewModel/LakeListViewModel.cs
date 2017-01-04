using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.Services;
using FishOn.Utils;
using Xamarin.Forms;

namespace FishOn.ViewModel
{
    public class LakeListViewModel : BaseViewModel
    {
        private Model.Lake _selectedLake;
        private string _newLakeNames;
        private int _waterTemp;

        ObservableCollection<Lake> _lakes;

        public LakeListViewModel(INavigation navigation) : base(navigation) { }
        public LakeListViewModel(INavigation navigation, ILakeDataService lakeDataService, ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService):base(navigation, lakeDataService, speciesDataService, wayPointDataService) { }
        
        public async Task Initialize()
        {
            var list = await _lakeService.GetLakesAsync();
            var observable = new ObservableCollection<Lake>(list);
            LakesList = observable;
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
                return new Command<Lake>((Lake selectedLake) =>
                {
                    SelectedLake = selectedLake;
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
