using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.MyFish
{
    public partial class MyFishTabbedPage : TabbedPage
    {
        private MyFishModelView _viewModel = null;
        public MyFishTabbedPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext != null)
            {
                _viewModel = (MyFishModelView) BindingContext;

                var pages = await _viewModel.GetContentPagesAsync();
                foreach (var page in pages)
                {
                    Children.Add(page);
                }
            }
        }
    }
}
