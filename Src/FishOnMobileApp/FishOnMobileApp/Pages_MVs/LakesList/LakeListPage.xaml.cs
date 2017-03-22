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
        private LakeListModelView _vm;
        private bool _appearProcess = false;

        public LakeListPage(LakeListModelView vm)
        {
            InitializeComponent();
            this.BindingContext = vm;
            _vm = vm;
           
            //Add buttons here in the constructor makes for a snappier ui
            AddLakeButtons();

            ImgBtnGenerator.AddButton(this.mainLayout, vm.SetSessionDataCommand, StyleSheet.Button_List_Width, StyleSheet.Small_Button_Height, "Start Trip");

            txtWaterTemp.Focus();
            ImgBtnGenerator.BindEntryEnterKeyPress_ToButtonClick(txtWaterTemp, vm.SetSessionDataCommand);
            
        }

        protected async override void OnAppearing()
        {
            if (!_appearProcess)
            {
                _appearProcess = true;
                this.CreateAddToolbarButton(_vm.AddNewLakes, async () => { AddLakeButtons(); });
            }
        }

        private void AddLakeButtons()
        {
            foreach (var lake in _vm.LakesList)
            {
                ImgBtnGenerator.AddButton(this.lakesBtnContainer, _vm.LakeSelectedCommand, StyleSheet.Button_List_Width, StyleSheet.Button_List_Height, lake.LakeName, "lake.png", StyleSheet.Species_Font_Size, StyleSheet.Button_BackColor);
            }
        }

    }
}
