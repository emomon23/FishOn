using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.ModelView;
using FishOn.Pages_MVs;
using FishOn.Utils;
using Xamarin.Forms;

namespace FishOn.ProvisioningPages.WayPoints
{
    public partial class WPProvisoningList : ContentPage
    {
         
        public WPProvisoningList(WayPointProvisioningModelView vm)
        {
            InitializeComponent();
            this.BindingContext = vm;
        }   
    }
}
