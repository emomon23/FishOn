using FishOn.ModelView;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FishOn.Pages_MVs.LakeMap
{
    public partial class LakeMapDetailPage : ContentPage
    {
        public LakeMapDetailPage(LakeMapPageModelView viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;
            viewModel.LakeMapControl = LakeMap;
        }

        public Map LakeMap
        {
            get { return wayPointMap; }
        }

        protected override async void OnAppearing()
        {
            if (fadeLabel.IsVisible)
            {
                await fadeLabel.FadeTo(0, 4000);
            }
        }
    }
}
