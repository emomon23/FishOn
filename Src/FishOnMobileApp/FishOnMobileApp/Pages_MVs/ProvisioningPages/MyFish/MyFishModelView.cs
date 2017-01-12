﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    }

}