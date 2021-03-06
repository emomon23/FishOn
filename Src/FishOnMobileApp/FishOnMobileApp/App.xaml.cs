﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FishOn.Pages_MVs;
using Xamarin.Forms;

namespace FishOnMobileApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new FishOnMobileApp.MainPage())
            {
                BarBackgroundColor = StyleSheet.NavigationPage_BarBackgroundColor,
                BarTextColor = StyleSheet.NavigationPage_BarTextColor
            };

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
