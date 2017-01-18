using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Utils;
using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.MyFish
{
    public partial class MyFishDetailPage : ContentPage
    {
        private MyFishModelView _modelView = null;

        public MyFishDetailPage()
        {
            InitializeComponent();

            ToolbarItems.Add(new ToolbarItem("Delete", null, async () =>
            {
                await _modelView.DeleteRecord_ClickAsync();
            }));

            ToolbarItems.Add(new ToolbarItem("  ", null, () =>
            {
              
            }));

            ToolbarItems.Add(new ToolbarItem("Save", null, async () =>
            {
                await _modelView.UpdateFishCaughtAsync();
            }));
          
        }

        protected async override void OnAppearing()
        {
            if (BindingContext != null)
            {
                _modelView = (MyFishModelView) BindingContext;
                SpeciesPicker.AddItems(_modelView.SpeciesNameList);
                WayPointPicker.AddItems(_modelView.WayPointNameList);
                LurePicker.AddItems(_modelView.LureNameList);

                SpeciesPicker.SelectedIndex = _modelView.SpeciesPickerSelectedIndex;
                WayPointPicker.SelectedIndex = _modelView.WayPointPickerSelectedIndex;
                LurePicker.SelectedIndex = _modelView.LurePickerSelectedIndex;
            }
        }
    }
}

