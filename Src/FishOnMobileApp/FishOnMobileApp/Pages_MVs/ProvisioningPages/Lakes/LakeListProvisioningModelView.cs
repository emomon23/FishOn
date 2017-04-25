using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.Model;
using FishOn.ModelView;
using FishOn.PlatformInterfaces;
using FishOn.Repositories;
using FishOn.Services;
using FishOn.Utils;
using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.Lakes
{
    public class LakeListProvisioningModelView :BaseModelView
    {
        private ObservableCollection<Lake> _lakes;

        public LakeListProvisioningModelView(INavigation navigation, ILakeDataService lakeDataService,
          ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService, IFishOnCurrentLocationService locationService, IAppSettingService appSettingService, IFishCaughtDataService fishCaughtDataService, ISessionDataService sessionDataService)
            : base(navigation, lakeDataService, speciesDataService, wayPointDataService, locationService, appSettingService, fishCaughtDataService,  sessionDataService) { }

        public async override Task InitializeAsync()
        {
            LakeList = new ObservableCollection<Lake>(await _lakeService.GetLakesAsync());
        }

        public ICommand EditLakeCommand
        {
            get
            {
                return new Command<Lake>(async (Lake lake) =>
                {
                    SimpleInputModalModelView vm = new SimpleInputModalModelView(_navigation, $"Edit Lake {lake.LakeName}", "Ok", "Cancel", lake.LakeName, true);

                    await vm.DisplayModalAsync(async (cancelClicked, valueProvided, delete) =>
                    {
                        if (delete)
                        {
                            if (await AreYouSureAsync("Delete Lake?"))
                            {
                                await _lakeService.DeleteAsync(lake);
                            }
                        }
                        else if (!cancelClicked && valueProvided.IsNotNullOrEmpty())
                        {
                            lake.LakeName = valueProvided;
                            await _lakeService.SaveAsync(lake);
                          
                        }

                        LakeList = new ObservableCollection<Lake>(await _lakeService.GetLakesAsync());
                    });
                });
            }
        }

        public ICommand AddLakesCommand
        {
            get
            {
                return new Command(async () =>
                {
                    SimpleInputModalModelView vm = new SimpleInputModalModelView(_navigation, "Add Lake(s).  Enter new lake names, seperated by a comma");

                    await vm.DisplayModalAsync(async (cancelClicked, valueProvided, dontMatter) =>
                    {
                        if (!cancelClicked && valueProvided.IsNotNullOrEmpty())
                        {
                            var newLakes = await _lakeService.CreateNewLakesAsync(valueProvided.Split(','));
                            if (newLakes != null && newLakes.Count > 0)
                            {
                                LakeList = new ObservableCollection<Lake>(newLakes);
                                OnPropertyChanged(nameof(LakeList));
                            }
                          
                        }
                    });
                });
            }
        }

        public ObservableCollection<Lake> LakeList
        {
            get { return _lakes; }
            set
            {
                _lakes = value;
                OnPropertyChanged();
            }
        }
    }
}
