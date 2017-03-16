using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.ModelView;
using FishOn.Pages_MVs;
using FishOn.Utils;
using Xamarin.Forms;

namespace FishOn
{
    public partial class SpeciesListPage : ContentPage
    {
        private SpeciesPageModelView _vm;

        public SpeciesListPage(SpeciesPageModelView vm)
        {
            InitializeComponent();

            _vm = vm;
            this.BindingContext = vm;

            var speciesList = _vm.SpeciesList;

            foreach (var species in speciesList)
            {
                ImgBtnGenerator.AddButton(speciesBtnContainer, _vm.SelectSpeciesCommand, StyleSheet.Button_List_Width,
                    StyleSheet.Button_List_Height, species.Name, species.imageIcon, StyleSheet.Species_Font_Size, StyleSheet.Button_BackColor);
            }
        }
    }
}
