using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace FishOn.Pages_VMs.ProvisioningPages.MyFish
{
    public partial class MyFishTabbedPage : TabbedPage
    {
        private MyFishViewModel _viewModel = null;
        public MyFishTabbedPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext != null)
            {
                _viewModel = (MyFishViewModel) BindingContext;

                var pages = await _viewModel.GetContentPagesAsync();
                foreach (var page in pages)
                {
                    Children.Add(page);
                }
            }
        }
    }
}
