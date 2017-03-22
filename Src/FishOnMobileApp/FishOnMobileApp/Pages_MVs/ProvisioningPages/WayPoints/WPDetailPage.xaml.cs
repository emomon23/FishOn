using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Model;
using FishOn.ModelView;
using FishOn.Utils;
using Xamarin.Forms;

namespace FishOn.ProvisioningPages.WayPoints
{
    public partial class WPDetailPage : ContentPage
    {
        public WPDetailPage(WayPointProvisioningModelView vm)
        {
            InitializeComponent();
            this.BindingContext = vm;

            WpTypePicker.AddItems(new string[] {WayPoint.WayPointTypeEnumeration.FishOn.ToString(), WayPoint.WayPointTypeEnumeration.BoatLaunch.ToString()});

            var lakeNames = vm.LakeList.Select(l => l.LakeName).ToArray();
            LakePicker.AddItems(lakeNames);
           
            ToolbarItems.Add(new ToolbarItem("Delete", null,  () =>
            {
                 vm.DeleteWayPointCommand.Execute(null);
            }));

            //space
            ToolbarItems.Add(new ToolbarItem("  ", null, () => {}));

            ToolbarItems.Add(new ToolbarItem("Save", null, () =>
            {
                vm.SaveWayPointCommand.Execute(null);
            }));
        }

        protected override async void OnAppearing()
        {}
    }
}
