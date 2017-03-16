using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.Settings
{
    public partial class SettingsListPage : ContentPage
    {
        public SettingsListPage(SettingsListViewModel vm)
        {
            InitializeComponent();
            this.BindingContext = vm;
        }
    }
}
