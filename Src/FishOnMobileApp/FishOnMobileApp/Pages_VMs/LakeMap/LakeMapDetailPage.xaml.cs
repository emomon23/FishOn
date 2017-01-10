using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FishOn.Pages_VMs.LakeMap
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
