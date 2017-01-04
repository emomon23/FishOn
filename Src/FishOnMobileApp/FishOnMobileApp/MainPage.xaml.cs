using System;
using System.Threading.Tasks;
using FishOn.ViewModel;
using Xamarin.Forms;

namespace FishOnMobileApp
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _pageViewModel = null;

        public MainPage()
        {
            InitializeComponent();
            _pageViewModel = new MainPageViewModel(this.Navigation);
            BindingContext = _pageViewModel;
        }

        protected override async void OnAppearing()
        {
            await _pageViewModel.Initialize();
        }

    }
}
