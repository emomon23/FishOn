using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.TackleBox
{
    public partial class TackleBoxPage : ContentPage
    {
        public TackleBoxPage(TackleBoxModelView vm)
        {
            InitializeComponent();
            this.BindingContext = vm;
        }
    }
}
