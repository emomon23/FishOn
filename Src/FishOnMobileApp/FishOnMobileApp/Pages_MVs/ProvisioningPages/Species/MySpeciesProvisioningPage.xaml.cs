using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.ModelView;
using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.Species
{
    public partial class MySpeciesProvisioningPage : ContentPage
    {
        public MySpeciesProvisioningPage()
        {
            InitializeComponent();
        }

        protected async void SpeciesFished_OnToggled(object sender, ToggledEventArgs e)
        {
            if (this.BindingContext != null)
            {
               var bc = (MySpeciestProvisioningViewModel) this.BindingContext;
               var species =  (Model.Species) ((Switch)sender).BindingContext;

               await bc.ToggleSelectedSpecies(species);
            }
        }
    }
}
