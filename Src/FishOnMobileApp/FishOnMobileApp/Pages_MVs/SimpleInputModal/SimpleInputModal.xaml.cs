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
    public partial class SimpleInputModal : ContentPage
    {
        public SimpleInputModal(SimpleInputModalModelView vm)
        {
            InitializeComponent();
            this.BindingContext = vm;

            if (vm.ShowDeleteButton)
            {
                ImgBtnGenerator.AddButton(btnContainer, vm.DeleteClick, StyleSheet.Small_Button_Width,
                    StyleSheet.Small_Button_Height, "Delete", "delete.ico",
                    backColor: StyleSheet.NavigationPage_BarBackgroundColor);
            }

            ImgBtnGenerator.AddButton(btnContainer, vm.CancelClick, StyleSheet.Small_Button_Width,
                   StyleSheet.Small_Button_Height, vm.CancelButtonText, "cancel.png",
                   backColor: StyleSheet.NavigationPage_BarBackgroundColor);

            ImgBtnGenerator.AddButton(btnContainer, vm.OkClick, StyleSheet.Small_Button_Width,
                   StyleSheet.Small_Button_Height, vm.OkButtonText, "ok.ico",
                   backColor: StyleSheet.NavigationPage_BarBackgroundColor);

            ImgBtnGenerator.BindEntryEnterKeyPress_ToButtonClick(txtValue, vm.OkClick);

        }

        protected async override void OnAppearing()
        {
            txtValue.Focus();
        }
    }
}
