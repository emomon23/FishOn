using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.MyFish
{
    public partial class MyFishByWayPoints : ContentPage
    {
        private MyFishModelView _modelView = null;

        public MyFishByWayPoints()
        {
            InitializeComponent();
        }

        protected async void FishOnSelectedAsync(Object sender, ItemTappedEventArgs e)
        {
            var fish = (Model.FishOn)(e.Item);
            await _modelView.EditFishOnAsync(fish);
           
        }

        protected override async void OnAppearing()
        {
            if (BindingContext != null)
            {
                _modelView = (MyFishModelView)BindingContext;
            }

            speciesHeaderListView.ItemsSource = null;
            speciesHeaderListView.ItemsSource = _modelView.FishCaughtByWayPoint;
        }
    }
}
