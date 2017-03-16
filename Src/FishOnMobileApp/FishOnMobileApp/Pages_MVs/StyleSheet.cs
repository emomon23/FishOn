using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FishOn.Pages_MVs
{
    public static class StyleSheet
    {
        public static Color LargeToggle_SelectedBackColor = Color.FromHex("91C498");
     
        public static int AccordionNodeContent_LabelHeight = 50;
        public static int AccordionNode_HeaderFontSize = 24;
        public static FontAttributes AccordNode_HeaderFontAttributes = FontAttributes.Bold;

        public static Color NavigationPage_BarBackgroundColor = Color.FromHex("67AF71");
        public static Color NavigationPage_BarTextColor = Color.White;

        public static Color Button_BackColor = Color.FromHex("67846C");
        public static Color Button_TextColor = Color.White;

        public static int Default_Label_Font_Size = 14;

        public static Color TabbedPage_TabFontColor = Color.White;

        public static Color ListView_OddRowBackColor = Color.Silver;
        public static Color ListView_EvenRowBackColor = Color.White;
        public static Color ListView_SelectedRowBackColor = Color.Gray;

        public static double Small_Button_Width = 125;
        public static double Small_Button_Height = 40;

        public static double Big_Button_Width = 275;
        public static double Big_Button_Height = 70;

        public static double Button_List_Width = 355;
        public static double Button_List_Height = 50;
        public static double Species_Font_Size = 24;

        public static int ModalDialog_LabelFontSize = 28;
        public static int ModalDialog_EntryFontSize = 20;
        public static Color ModalDialog_LabelBackColor = NavigationPage_BarBackgroundColor;
        public static Color ModalDialog_EntryTextColor = Color.Black;
        public static Color ModalDialog_LabelTextColor = NavigationPage_BarTextColor;

    }
}
