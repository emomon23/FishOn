using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.Services;
using Xamarin.Forms;

namespace FishOn.ViewModel
{
    public class MyDataListViewModel : BaseViewModel
    {
        public MyDataListViewModel(INavigation navigation) : base(navigation) { }
        public MyDataListViewModel(INavigation navigation, ILakeDataService lakeDataService, ISpeciesDataService speciesDataService, IWayPointDataService wayPointDataService):base(navigation, lakeDataService, speciesDataService, wayPointDataService) { }

        public ICommand LakesButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    
                });
            }
        }

        public ICommand WayPointsCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Navigate_To_WayPointProvisoning_MasterDetailPageAsync();
                });
            }
        }

        public ICommand SpeciesCommand
        {
            get
            {
                return new Command(() =>
                {

                });
            }
        }

        public ICommand FishOnCommand
        {
            get
            {
                return new Command(() =>
                {

                });
            }
        }

        public ICommand LuresCommand
        {
            get
            {
                return new Command(() =>
                {

                });
            }
        }

        public ICommand LakesCommand
        {
            get
            {
                return new Command(() =>
                {

                });
            }
        }
    }
}
