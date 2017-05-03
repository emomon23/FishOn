using FishOn.ModelView;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FishOn.Pages_MVs.LakeMap
{
    public partial class LakeMapMasterDetailPage : MasterDetailPage
    {
        private Map _map;

        public LakeMapMasterDetailPage(LakeMapPageModelView vm)
        {
            InitializeComponent();
            this.BindingContext = vm;
            var detailPage = new LakeMapDetailPage(vm);
            _map = detailPage.LakeMap;

            // For Android & Windows Phone, provide a way to get back to the master page.
            if (Device.OS != TargetPlatform.iOS)
            {
                TapGestureRecognizer tap = new TapGestureRecognizer();
                tap.Tapped += (sender, args) => {
                    this.IsPresented = true;
                };

                detailPage.Content.BackgroundColor = Color.Transparent;
                detailPage.Content.GestureRecognizers.Add(tap);
            }

            Detail = detailPage;
            Master = new LakeMapMasterPage(vm);
        }

        public Map WayPointsMap
        {
            get { return _map; }
        }
    }
}
