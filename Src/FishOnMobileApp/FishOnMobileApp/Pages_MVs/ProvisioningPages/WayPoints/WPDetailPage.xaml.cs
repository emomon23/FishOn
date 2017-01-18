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
        private WayPointProvisioningModelView _modelView;

        public WPDetailPage()
        {
            InitializeComponent();
            WpTypePicker.AddItems(new string[] {WayPoint.WayPointTypeEnumeration.FishOn.ToString(), WayPoint.WayPointTypeEnumeration.BoatLaunch.ToString()});

           
            ToolbarItems.Add(new ToolbarItem("Delete", null,  () =>
            {
                 _modelView.DeleteWayPointCommand.Execute(null);
            }));

            //space
            ToolbarItems.Add(new ToolbarItem("  ", null, () => {}));

            ToolbarItems.Add(new ToolbarItem("Save", null, () =>
            {
                _modelView.SaveWayPointCommand.Execute(null);
            }));
        }

        protected override async void OnAppearing()
        {
            if (BindingContext != null)
            {
                _modelView = (WayPointProvisioningModelView) BindingContext;
            }
        }
    }
}
