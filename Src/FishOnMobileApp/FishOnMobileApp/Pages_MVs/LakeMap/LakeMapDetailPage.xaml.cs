using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FishOn.Pages_MVs.LakeMap
{
    public partial class LakeMapDetailPage : ContentPage
    {
        public LakeMapDetailPage()
        {
            InitializeComponent();
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
