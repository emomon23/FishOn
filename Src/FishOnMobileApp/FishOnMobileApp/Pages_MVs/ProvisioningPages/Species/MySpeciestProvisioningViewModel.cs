using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.ModelView;
using FishOn.Pages_MVs.AccordionViewModel;
using FishOn.PlatformInterfaces;
using FishOn.Repositories;
using FishOn.Services;
using FishOn.Utils;
using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.Species
{
    public class MySpeciestProvisioningViewModel : BaseModelView
    {
        private ObservableCollection<Model.Species> _availableSpecies;

        public MySpeciestProvisioningViewModel(INavigation navigation) : base(navigation) { }

        public MySpeciestProvisioningViewModel(INavigation navigation, ILakeDataService lakeDataService,
            ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService,
            IFishOnCurrentLocationService locationService, IAppSettingService appSettingService, IFishCaughtDataService fishCaughtDataService,  ISessionDataService sessionDataService)
            : base(
                navigation, lakeDataService, speciesDataService, wayPointDataService, locationService, appSettingService, fishCaughtDataService, sessionDataService){}

        public async override Task InitializeAsync()
        {
            await base.InitializeAsync();
            RefreashSpeciesList();
        }

         public ObservableCollection<Model.Species> AvailableSpecies
        {
            get
            {
                return _availableSpecies;
            }
            set
            {
                _availableSpecies = value;
                OnPropertyChanged();
            }
        }

        public ICommand EditSpeciesCommand
        {
            get
            {
                return new Command<Model.Species>(async (Model.Species speciestoEdit) =>
                {
                    var modalDialogViewModal = new SimpleInputModalModelView(_navigation,
                     "Edit Species", defaultValue: speciestoEdit.Name, showDeleteButton:true);

                    await modalDialogViewModal.DisplayModalAsync(
                        async (bool cancelClicked, string speciesName, bool deleteClicked) =>
                        {
                            if (deleteClicked && await AreYouSureAsync($"Delete {speciestoEdit.Name}?"))
                            {
                                await _speciesDataService.DeleteAsync(speciestoEdit);
                                AvailableSpecies.Remove(speciestoEdit);
                            }
                            else if (!cancelClicked && speciesName.IsNotNullOrEmpty() && speciesName != speciestoEdit.Name)
                            {
                                speciestoEdit.Name = speciesName;
                                await _speciesDataService.SaveAsync(speciestoEdit);
                                RefreashSpeciesList();
                            }
                        });
                });
            }
        }

        public ICommand AddNewSpecies
        {
            get
            {
                return new Command(async () => {
                    var modalDialog = new SimpleInputModalModelView(_navigation, "Create New Species");

                    await modalDialog.DisplayModalAsync(
                        async (bool cancelClicked, string speciesName, bool deleteClickedCantHappend) =>
                        {
                            if (!cancelClicked && ! AvailableSpecies.Any(s => s.Name.Equals(speciesName, StringComparison.CurrentCultureIgnoreCase)))
                            {
                                var newSpcies = new Model.Species()
                                {
                                    Name = speciesName,
                                    DisplayOrder = AvailableSpecies.Count * 10,
                                    IsAvailableOnCatchList = true
                                };

                                await _speciesDataService.SaveAsync(newSpcies);
                                await RefreashSpeciesList();
                            }
                        });
                });
            }
        }

        public async Task ToggleSelectedSpecies(Model.Species species)
        {
            _speciesDataService.SaveAsync(species);
        }

        private async Task RefreashSpeciesList()
        {
            var speciesList = await _speciesDataService.GetSpeciesAsync();
            AvailableSpecies = new ObservableCollection<Model.Species>(speciesList);
        }
    }
}
