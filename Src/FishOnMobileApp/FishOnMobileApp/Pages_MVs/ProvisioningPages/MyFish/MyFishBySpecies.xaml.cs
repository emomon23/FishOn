using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.MyFish
{
    public partial class MyFishBySpecies : ContentPage
    {
        public MyFishBySpecies()
        {
            InitializeComponent();
        }

        protected async void FishOnSelectedAsync(Object sender, ItemTappedEventArgs e)
        {
            if (BindingContext != null)
            {
                var modelView = (MyFishModelView) BindingContext;
                var fish = (Model.FishOn) (e.Item);

                await modelView.EditFishOnAsync(fish);

            }
        }
    }
}
