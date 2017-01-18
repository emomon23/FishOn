using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.Model.ViewModel;
using FishOn.PlatformInterfaces;
using FishOn.Services;
using FishOn.ModelView;
using FishOn.Pages_MVs.AccordionViewModel;
using FishOn.Utils;
using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.MyFish
{
    public class MyFishModelView : BaseModelView
    {
        private List<Page> _pages = new List<Page>();
        private List<WayPoint> _wayPoints;
        private List<Model.Species> _species;

        private ObservableCollection<FishOnGroupBySpeciesViewModel> _speciesCaught = null;
        private List<Model.Species> _allAvailableSpecies = null;
        private List<FishingLure> _availableLures = null;
        private ObservableCollection<FishOnGroupByWayPointViewModel> _wayPointFishCaught = null;


        public MyFishModelView(INavigation navigation) : base(navigation) { }

        public MyFishModelView(INavigation navigation, ILakeDataService lakeDataService,
            ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService,
            IFishOnCurrentLocationService locationService, IAppSettingService appSettingService, IFishCaughtDataService fishCaughtDataService)
            : base(
                navigation, lakeDataService, speciesDataService, wayPointDataService, locationService, appSettingService, fishCaughtDataService
            )
        {
            LureAccordion = new AccordionNodeViewModel("Fishing Lure", 300, Color.Blue, Color.White, Color.Black, expandInitially:true);
            WeatherAccordion = new AccordionNodeViewModel("Weather Conditions", 350, Color.Blue, Color.White, Color.Black);
        }

        public string Title
        {
            get { return "My Fish"; }
        }

        public async Task<List<Page>> GetContentPagesAsync()
        {
            if (_pages.Count == 0)
            {
                _wayPoints = await _wayPointDataService.GetWayPointsAsync();
                _allAvailableSpecies = await _speciesDataService.GetSpeciesAsync();
                _availableLures = await _fishOnDataService.GetAvailableLuresAsync();

                _pages.Add(new MyFishByWayPoints() {BindingContext = this});
                _pages.Add(new MyFishBySpecies() {BindingContext = this});
            }

            return _pages;
        }

       
        public int LurePickerSelectedIndex
        {
            get
            {
                var index = 0;
                if (!(FishCaught == null || FishCaught.Lure == null || FishCaught.Lure.FishingLureId == 0))
                {
                    index = _availableLures.FindIndex(l => l.FishingLureId == FishCaught.Lure.FishingLureId) + 1;
                }

               
                return index;
            }
            set
            {
                if (value == 0)
                {
                    FishCaught.Lure = new FishingLure();
                    LureAccordion.ShowSubContent();
                }
                else
                {
                    FishCaught.Lure = _availableLures[value -1];
                    FishCaught.FishingLureId = FishCaught.Lure.FishingLureId;
                    LureAccordion.HideSubContent(50);
                }

                OnPropertyChanged(nameof(FishCaught));
            }
        }

        public int WayPointPickerSelectedIndex
        {
            get { return _wayPoints.FindIndex(w => w.WayPointId == FishCaught.WayPointId); }
            set
            {
                var wayPoint = _wayPoints[value];
                if (wayPoint.WayPointId != FishCaught.WayPointId)
                {
                    FishCaught.WayPointId = wayPoint.WayPointId;
                    FishCaught.WayPoint = wayPoint;
                    FishCaughtIsDirty = true;
                }
            }
        }

        public int SpeciesPickerSelectedIndex
        {
            get { return _allAvailableSpecies.FindIndex(s => s.SpeciesId == FishCaught.SpeciesId); }
            set
            {
                var species = _allAvailableSpecies[value];
                if (species.SpeciesId != FishCaught.SpeciesId)
                {
                    FishCaught.SpeciesId = species.SpeciesId;
                    FishCaught.Species = species;
                    FishCaughtIsDirty = true;
                }
            }
        }

        public bool FishCaughtIsDirty { get; private set; }

        public string [] SpeciesNameList
        {
            get
            {
                var result = (from s in _allAvailableSpecies
                              select s.Name).ToArray();

                return result;
            }
            
        }

        public string[] LureNameList
        {
            get
            {
                var result = (from l in _availableLures
                    select l.LureDescriptionSummary).ToList();

                result.Insert(0, "NEW LURE");
                
                return result.ToArray();
            }
        }

        public string [] WayPointNameList
        {
            get
            {
                var wpNames = (from w in _wayPoints
                    select w.Name).Distinct().ToArray();

                return wpNames;
            }
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
            OnPropertyChanged(nameof(LurePickerSelectedIndex));
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

        public async Task DeleteRecord_ClickAsync()
        {
            var answer = await AreYouSureAsync("Delete FishOn Record?");

            if (answer)
            {
                await _fishOnDataService.DeleteFishCaughtAsync(FishCaught);
                _wayPointDataService.RemoveFishFromCache(FishCaught);
                Navigate_ToMyDataButtonsListAsync();
            }
        }
        
        public async Task UpdateFishCaughtAsync()
        {
            var lure = FishCaught.Lure;
            bool addLureToList = lure.FishingLureId == 0 && lure.IsValid;

            await _fishOnDataService.UpdateFishCaughtAsync(FishCaught);

            if (addLureToList)
            {
                _availableLures.Add(FishCaught.Lure);
            }

            if (FishCaught.WayPointIsDirty)
            {
                _wayPointFishCaught = null;
                _wayPointDataService.UpdateCache(FishCaught);
                _wayPoints = await _wayPointDataService.GetWayPointsAsync();
                
                OnPropertyChanged(nameof(FishCaughtByWayPoint));
            }

            if (FishCaught.SpeciesIsDirty)
            {
                _speciesCaught = null;
                OnPropertyChanged(nameof(FishCaughtBySpecies));
            }

            FishCaught.ResetDirtyFlags();
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


        public AccordionNodeViewModel LureAccordion { get; private set; }
        public AccordionNodeViewModel WeatherAccordion { get; private set; }
   }

}
