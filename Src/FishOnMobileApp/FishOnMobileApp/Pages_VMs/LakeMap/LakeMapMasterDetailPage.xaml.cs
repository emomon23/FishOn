﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FishOn.Pages_VMs.LakeMap
{
    public partial class LakeMapMasterDetailPage : MasterDetailPage
    {
        private Map _map;

        public LakeMapMasterDetailPage()
        {
            InitializeComponent();
            
            var detailPage = new LakeMapDetailPage();
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
            Master = new LakeMapMasterPage();
        }

        public Map WayPointsMap
        {
            get { return _map; }
        }
    }
}
