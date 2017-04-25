using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.PlatformInterfaces;
using FishOn.Utils;
using FishOn.VoiceRecognitionSystem;
using Xamarin.Forms;

namespace FishOn.Pages_MVs.WaypointNameMethodRecorder
{
    public partial class VoiceToTextPage : ContentPage
    {
        private FieldSetGenerator fieldSetGenerator;
        private WaypointNameMethodRecorderViewModel _vm;

        public VoiceToTextPage(WaypointNameMethodRecorderViewModel vm)
        {
            this._vm = vm;
            this.BindingContext = vm;
            InitializeComponent();
            CreateFieldSets();
        }

        private void CreateFieldSets()
        {
            mainContentLayout.BindingContext = _vm.NewFishOn;
            fieldSetGenerator = new FieldSetGenerator(this.mainContentLayout);

           // fieldSetGenerator.CreateVoiceToTextFieldSet("wayPointName", "Way Point Name", "WayPointName");
            fieldSetGenerator.CreateAutoCompleteFieldSet(new AutoCompleteDefinition()
            {
                Identifier = "wayPointName",
                Binding = "WayPointName",
                DataSource = _vm.WayPointList.Select(w => w.Name).ToList(),
                IncludeVoiceToTextButton = true,
                LabelText = "Way Point Name",
            });


            fieldSetGenerator.CreateVoiceToTextFieldSet("fishingMethod", "Fishing Method", "FishingMethod");
             
            fieldSetGenerator.CreateAutoCompleteFieldSet(new AutoCompleteDefinition()
            {
                Identifier = "lake",
                LabelText = "Lake",
                Binding = "LakeName",
                DataSource = _vm.AvailableLakes.Select(l => l.LakeName).ToList(),
                IncludeVoiceToTextButton = true
            });

            fieldSetGenerator.CreateFieldSet("waterTemp", "Water Temp", "WaterTemp");

            fieldSetGenerator.CreateFieldSet("weatherCondition", "Current Conditions", "Conditions");
            fieldSetGenerator.CreateFieldSet("dateCaught", "Date Caught", "DateCaught");
            fieldSetGenerator.CreateFieldSet("moonPhase", "Moon Phone", "MoonPhase");

            this.CreateCancelButton(_vm.Cancel);

            this.CreateSaveToolbarButton(async () =>
            {
                DisplayAlert("Save Clicked", _vm.NewFishOn.WayPointName, "OK");
            });
        }
    }
}
