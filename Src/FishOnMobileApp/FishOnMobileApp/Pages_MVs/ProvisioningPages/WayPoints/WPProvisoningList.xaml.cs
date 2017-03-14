using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Pages_MVs;
using FishOn.Utils;
using Xamarin.Forms;

namespace FishOn.ProvisioningPages.WayPoints
{
    public partial class WPProvisoningList : ContentPage
    {
        ListViewAlternatingRowProcessor _listViewProcessor= new ListViewAlternatingRowProcessor(StyleSheet.ListView_EvenRowBackColor, StyleSheet.ListView_OddRowBackColor, StyleSheet.ListView_SelectedRowBackColor);
        
        public WPProvisoningList()
        {
            InitializeComponent();
        }

        private void Cell_OnAppearing(object sender, EventArgs e)
        {
            _listViewProcessor.SetBackColor(sender);
        }
    }
}
