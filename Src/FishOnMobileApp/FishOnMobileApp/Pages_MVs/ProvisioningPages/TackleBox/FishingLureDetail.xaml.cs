using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.TackleBox
{
    public partial class FishingLureDetail : ContentPage
    {
        private TackleBoxModelView _modelView = null;

        public FishingLureDetail()
        {
            InitializeComponent();
            
            ToolbarItems.Add(new ToolbarItem("Delete", null, async () =>
            {
                await _modelView.DeleteLure();
            }));

            ToolbarItems.Add(new ToolbarItem("  ", null, () =>
            {

            }));

            ToolbarItems.Add(new ToolbarItem("Save", null, async () =>
            {
                await _modelView.SaveLure();
            }));
        }

        protected override void OnAppearing()
        {
            if (_modelView == null)
            {
                _modelView = (TackleBoxModelView)BindingContext;
            }
        }
    }
}
