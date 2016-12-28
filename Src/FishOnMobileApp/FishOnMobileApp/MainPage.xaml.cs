using FishOn.ViewModel;
using Xamarin.Forms;

namespace FishOnMobileApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(this.Navigation);
        }
    }
}
