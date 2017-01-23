﻿using System;
using System.Threading.Tasks;
using FishOn.ModelView;
using Xamarin.Forms;
using System.Reflection;

namespace FishOnMobileApp
{
    public partial class MainPage : ContentPage
    {
        private MainPageModelView _pageViewModel = null;

        public MainPage()
        {
            InitializeComponent();
            _pageViewModel = new MainPageModelView(this.Navigation);
          

            BindingContext = _pageViewModel;
            
        }

        protected override async void OnAppearing()
        {
            if (_pageViewModel != null)
            {
                await _pageViewModel.InitializeAsync();
            }
        }

    }
}
