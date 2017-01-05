using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FishOn
{
    public partial class LakeMapPage : ContentPage
    {
        public LakeMapPage()
        {
            InitializeComponent();
        }

        public Map LakeMapControl
        {
            get { return WayPointsMap; }
        }
    }
}
