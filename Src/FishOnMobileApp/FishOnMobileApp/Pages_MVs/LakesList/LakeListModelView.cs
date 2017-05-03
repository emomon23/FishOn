using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.PlatformInterfaces;
using FishOn.Repositories;
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
        private ISessionDataService _sessionDataService;
        ObservableCollection<Lake> _lakes;

        public LakeListModelView(FishOnNavigationService navigation, IFishOnService fishOnService, ISessionDataService sessionData) : base(navigation, fishOnService)
        {
        }
        
        public async override Task InitializeAsync()
        {
            var list = await _fishOnService.GetLakesAsync();
            var observable = new ObservableCollection<Lake>(list);
            LakesList = observable;

            if (_sessionDataService.CurrentLake != null)
            {
                SelectedLake = _lakes.SingleOrDefault(l => l.LakeId == _sessionDataService.CurrentLake.LakeId);
            }

            WaterTemp = _sessionDataService.CurrentWaterTemp;
        }

        public ObservableCollection<Lake> LakesList
        {
            get { return _lakes; }
            private set
            {
                _lakes = value;
                OnPropertyChanged();
            }
        }

        public string NewLakeNamesSeperatedByComma
        {
            get { return _newLakeNames; }
            set
            {
                _newLakeNames = value;
                OnPropertyChanged();
            }
        }

        public Lake SelectedLake
        {
            get { return _selectedLake; }
            set
            {
                _selectedLake = value;
                OnPropertyChanged();
            }
        }

        public int WaterTemp
        {
            get { return _waterTemp; }
            set
            {
                _waterTemp = value;
                OnPropertyChanged();
            }
        }

        public async Task AddNewLakes(Func<Task> postSaveCallback)
        {
            var simpleViewVM = new SimpleInputModalModelView(_navigation,
                "Enter as many lake names, seperated by a comma, as you like", "Save");

            await simpleViewVM.DisplayModalAsync(
                async (bool cancelClicked, string lakeNamesProvided, bool deleteclicked) =>
                {
                    if (!cancelClicked)
                    {
                        var lakes = await _fishOnService.CreateNewLakesAsync(lakeNamesProvided.Split(','));
                        LakesList = new ObservableCollection<Lake>(lakes);
                        await postSaveCallback();
                    }
                });
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
                return new  Command(async () =>
                {
                    _sessionDataService.CurrentLake = SelectedLake;
                    _sessionDataService.CurrentWaterTemp = _waterTemp;
                    await _navigation.Navigate_BackToLandingPageAsync();
                });
            }
        }
    }
}
