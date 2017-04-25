using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.Model;
using FishOn.PlatformInterfaces;
using FishOn.Services;
using FishOn.ModelView;
using FishOn.Repositories;
using FishOn.Utils;
using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.TackleBox
{
    public class TackleBoxModelView : BaseModelView
    {
        private ObservableCollection<FishingLure> _fishingLures = null;

        public TackleBoxModelView(INavigation navigation, ILakeDataService lakeDataService,
           ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService, IFishOnCurrentLocationService locationService, IAppSettingService appSettingService, IFishCaughtDataService fishCaughtDataService, ISessionDataService sessionDataService)
            : base(navigation, lakeDataService, speciesDataService, wayPointDataService, locationService, appSettingService, fishCaughtDataService, sessionDataService){ }

        public async override Task InitializeAsync()
        {
            FishingLures = new ObservableCollection<FishingLure>(await _fishOnDataService.GetAvailableLuresAsync(true));
        }

        public FishingLure Lure { get; private set; }

        public ICommand EditLureCommand
        {
            get
            {
                return new Command<FishingLure>(async (FishingLure lure) =>
                {
                    this.Lure = lure;
                    var lureDetail = new FishingLureDetail();
                    lureDetail.BindingContext = this;
                    await _navigation.PushAsync(lureDetail);
                });
            }
        }

        public ObservableCollection<FishingLure> FishingLures
        {
            get { return _fishingLures; }
            set
            {
                _fishingLures = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddNewLureCommand
        {
            get
            {
                return new Command(async () =>
                {
                    Lure=new FishingLure();
                    var lureDetail = new FishingLureDetail();
                    lureDetail.BindingContext = this;
                    await _navigation.PushAsync(lureDetail);
                });
            }
        }

        public async Task DeleteLure()
        {
            if (await AreYouSureAsync($"Delete Fishing Lure {Lure.LureDescriptionSummary}?"))
            {
                await _fishOnDataService.DeleteLureAsync(Lure);
            }

           _navigation.GoBackOnePage();
        }

        public async Task SaveLure()
        {
            await _fishOnDataService.SaveLureAsync(Lure);
            _navigation.GoBackOnePage();
        }
    }
}
