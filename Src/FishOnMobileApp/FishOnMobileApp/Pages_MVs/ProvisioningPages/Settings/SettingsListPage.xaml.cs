using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Utils;
using Xamarin.Forms;

namespace FishOn.Pages_MVs.ProvisioningPages.Settings
{
    public partial class SettingsListPage : ContentPage
    {
        public SettingsListPage(SettingsListViewModel vm)
        {
            InitializeComponent();
            this.BindingContext = vm;
           
            foreach (var pageDefinition in vm.PageDefinitions)
            {
                ImgBtnGenerator.AddButton(btnContainer, vm.DisplayProvisioningPage, StyleSheet.Button_List_Width, StyleSheet.Button_List_Height, pageDefinition.PageTitle, "expand_right.png", StyleSheet.Default_Label_Font_Size, StyleSheet.Button_BackColor, AlignImageEnumeration.Right);
            }
        }
    }
}
