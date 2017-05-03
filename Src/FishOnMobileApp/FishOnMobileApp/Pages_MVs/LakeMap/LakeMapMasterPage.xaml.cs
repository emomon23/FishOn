using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.ModelView;
using Xamarin.Forms;

namespace FishOn.Pages_MVs.LakeMap
{
    public partial class LakeMapMasterPage : ContentPage
    {
        public LakeMapMasterPage(LakeMapPageModelView vm) 
        {
            InitializeComponent();
            this.BindingContext = vm;
        }

        //Xamarin switch doesn't support binding, this is a work around, until I find / implement a better solution
        protected void FilterToggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            var binding = this.BindingContext;
            var species = ((Switch) sender).BindingContext;

            if (binding != null)
            {
                ((LakeMapPageModelView) binding).SpeciesFilterSwitch((Species) species);
            }
        }
    }
}
