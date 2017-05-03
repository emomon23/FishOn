using System;
using System.Threading.Tasks;
using FishOn.ModelView;
using Xamarin.Forms;
using System.Reflection;
using FishOn.Pages_MVs;
using FishOn.Services;
using FishOn.Utils;

namespace FishOnMobileApp
{
    public partial class MainPage : ContentPage
    {
        private MainPageModelView _pageViewModel = null;
        private FishOnNavigationService _navigationService = null;

        public MainPage()
        {
            InitializeComponent();
            _navigationService = new FishOnNavigationService(this.Navigation);
        }

        protected override async void OnAppearing()
        {
            _pageViewModel = await _navigationService.CreateMainPageViewModel(Navigation);

            BindingContext = _pageViewModel;

            ImgBtnGenerator.AddButton(logoContainer, _pageViewModel.FishOnCommand, 140, 140, image: "fish_on_logo.png");

            ImgBtnGenerator.AddButton(findSpotPanel, _pageViewModel.FindMySpotCommand,
                                                StyleSheet.Big_Button_Width, StyleSheet.Big_Button_Height,
                                                "Find Spot", "find_it.png", 23);


            ImgBtnGenerator.AddButton(smallButtonsContainer, _pageViewModel.SetLakeWaterTemp,
                                                StyleSheet.Small_Button_Width, StyleSheet.Small_Button_Height,
                                                "Lake", "lake.png", 14);


            ImgBtnGenerator.AddButton(smallButtonsContainer, _pageViewModel.MyDataCommand,
                                                StyleSheet.Small_Button_Width, StyleSheet.Small_Button_Height,
                                                "Settings", "settings.png", 14);

        }

    }
}
