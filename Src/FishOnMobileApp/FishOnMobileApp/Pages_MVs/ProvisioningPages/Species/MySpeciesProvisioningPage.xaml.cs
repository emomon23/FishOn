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
        private MySpeciestProvisioningViewModel _vm;
        public MySpeciesProvisioningPage(MySpeciestProvisioningViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            this.BindingContext = vm;
        }

        protected async void SpeciesFished_OnToggled(object sender, ToggledEventArgs e)
        {
            var species = (Model.Species)((Switch)sender).BindingContext;
            await _vm.ToggleSelectedSpecies(species);
        }
    }
}
