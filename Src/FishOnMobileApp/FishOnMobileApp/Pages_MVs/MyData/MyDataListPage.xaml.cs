using FishOn.ViewModel;
using Xamarin.Forms;

namespace FishOn.Pages_VMs.MyData
{
    public partial class MyDataListPage : TabbedPage
    {
        private MyDataListViewModel _vm = null;

        public MyDataListPage()
        {
            InitializeComponent();

            CurrentPageChanged += async (sender, args) =>
            {
                if (_vm != null)
                {
                    await _vm.InitializeNewPageContextAsync(CurrentPage.Title);
                }
            };
        }

        protected override async  void OnAppearing()
        {
            base.OnAppearing();
            
            if (Children.Count == 0)
            {
                _vm = (MyDataListViewModel) BindingContext;

                if (_vm != null)
                {
                    foreach (var childPage in _vm.GetContentPages())
                    {
                        Children.Add(childPage);
                    }
                }
            }
            
        }

        
    }
}
