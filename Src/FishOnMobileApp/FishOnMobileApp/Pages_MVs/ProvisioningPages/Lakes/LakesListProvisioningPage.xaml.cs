﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.Lakes
{
    public partial class LakesListProvisioningPage : ContentPage
    {
        private LakeListProvisioningModelView _vm;
        public LakesListProvisioningPage(LakeListProvisioningModelView vm)
        {
            InitializeComponent();
            _vm = vm;
            this.BindingContext = vm;
        }
    }
}
