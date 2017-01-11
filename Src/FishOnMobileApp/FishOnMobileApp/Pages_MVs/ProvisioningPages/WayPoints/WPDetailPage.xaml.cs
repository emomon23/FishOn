using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.Utils;
using Xamarin.Forms;

namespace FishOn.ProvisioningPages.WayPoints
{
    public partial class WPDetailPage : ContentPage
    {
        public WPDetailPage()
        {
            InitializeComponent();
            WpTypePicker.AddItems(new string[] {WayPoint.WayPointTypeEnumeration.FishOn.ToString(), WayPoint.WayPointTypeEnumeration.BoatLaunch.ToString()});

        }
    }
}
