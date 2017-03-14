using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.ModelView;
using FishOn.Pages_MVs;
using FishOn.Utils;
using Xamarin.Forms;

namespace FishOn
{
    public partial class LakeListPage : ContentPage
    {
        public LakeListPage(LakeListModelView vm)
        {
            InitializeComponent();
            this.BindingContext = vm;
            foreach (var lake in vm.LakesList)
            {
                ImgBtnGenerator.AddButton(this.lakesBtnContainer, vm.LakeSelectedCommand, StyleSheet.Species_Button_Width, StyleSheet.Species_Button_Height, lake.LakeName, "lake.png", StyleSheet.Species_Font_Size, StyleSheet.Button_BackColor);
            }

            ImgBtnGenerator.AddButton(this.mainLayout, vm.SetSessionDataCommand, StyleSheet.Species_Button_Width, StyleSheet.Small_Button_Height, "Start Trip");
        }
    }
}
