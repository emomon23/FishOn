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
            CreateUIControls();
        }

        private void CreateUIControls()
        {
            mainContentLayout.BindingContext = _vm.NewFishOn;
            fieldSetGenerator = new FieldSetGenerator(this.mainContentLayout);

            fieldSetGenerator.CreateAutoCompleteFieldSet(new AutoCompleteDefinition()
            {
                Identifier = "wayPointName",
                Binding = "WayPointName",
                DataSource = _vm.WayPointList.Select(w => w.Name).ToList(),
                IncludeVoiceToTextButton = true,
                LabelText = "Way Point Name",
                VoiceToTextConverstion = (value) =>
                {
                    if (_vm.IsLakeNameInWayPointName)
                    {
                        fieldSetGenerator.UpdateEntryText("lake", _vm.ParseOutLakeNameFromWayPointName());
                        fieldSetGenerator.UpdateEntryText("wayPointName", _vm.WayPointNameWithLakeParseOut);
                    }
                }
            });


            fieldSetGenerator.CreateVoiceToTextFieldSet("fishingMethod", "Fishing Method", "FishingMethod");
             
            //START FLOAT
            fieldSetGenerator.StartFloat();
                fieldSetGenerator.CreateAutoCompleteFieldSet(new AutoCompleteDefinition()
                {
                    Identifier = "lake",
                    LabelText = "Lake",
                    Binding = "LakeName",
                    DataSource = _vm.AvailableLakes.Select(l => l.LakeName).ToList(),
                }, 65);

                fieldSetGenerator.CreateFieldSet("waterTemp", "Water Temp", "WaterTemp");
            fieldSetGenerator.EndFloat();

            fieldSetGenerator.CreateFieldSet("weatherCondition", "Current Conditions", "Conditions" );

            //START FLOAT
            fieldSetGenerator.StartFloat();
                fieldSetGenerator.CreateFieldSet("dateCaught", "Date Caught", "DateCaught", floatWithPercent:65);
                fieldSetGenerator.CreateFieldSet("moonPhase", "Full Moon", "MoonPhase");
            fieldSetGenerator.EndFloat();

           
            var cameraControl = ImgBtnGenerator.AddButton(cameraButtonContainer, CameraClick, StyleSheet.Small_Button_Width,
                StyleSheet.Small_Button_Height, null, "camera.png", null, Color.Transparent);
            cameraControl.HorizontalOptions = LayoutOptions.Center;
           
            this.CreateCancelButton(_vm.Cancel);

            this.CreateSaveToolbarButton(async () =>
            {
                DisplayAlert("Save Clicked", _vm.NewFishOn.WayPointName, "OK");
            });
        }

        private async Task CameraClick()
        {
            var result = await _vm.TakePicture();

            if (_vm.NewFishOn.Image1FileName.IsNotNullOrEmpty())
            {
                image1.Source = _vm.NewFishOn.Image1FileName;
                image1.BackgroundColor = StyleSheet.NavigationPage_BarBackgroundColor;
            }

            if (_vm.NewFishOn.Image2FileName.IsNotNullOrEmpty())
            {
                image2.Source = _vm.NewFishOn.Image2FileName;
                image2.BackgroundColor = StyleSheet.NavigationPage_BarBackgroundColor;
            }
        }
    }
}
