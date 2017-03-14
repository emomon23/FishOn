using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Model.ViewModel;
using FishOn.Pages_MVs.AccordionViewModel;
using FishOn.Utils;
using Xamarin.Forms;
using FishOn = FishOn.Model.FishOn;

namespace FishOn.Pages_MVs.ProvisioningPages.MyFish
{
    public partial class MyFishList : ContentPage
    {
        private MyFishListModelView _viewModel;

        public MyFishList()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (this.BindingContext != null)
            {
                _viewModel = (MyFishListModelView)this.BindingContext;
                await _viewModel.InitializeAsync();
            }

            AddToggleButtons();
            AddAccordions();
        }

        private void AddAccordions()
        {

            if (byWayPointLayout.Children.Count > 0)
            {
                return;
            }

            foreach (var wayPoint in _viewModel.FishCaughtByWayPoint)
            {
                var fishListView = CreateFishListView(wayPoint);
                var acWayPointNode = AccordionFactory.CreateNewNode(
                    new AcNodeConfiguration()
                    {
                        HeaderText = wayPoint.WayPointName,
                        HeaderBackGroundColor = StyleSheet.Button_BackColor,
                        HeaderTextColor = StyleSheet.Button_TextColor,
                        FontSize = StyleSheet.AccordionNode_HeaderFontSize,
                        HeaderFontAttributes = StyleSheet.AccordNode_HeaderFontAttributes,
                        ExpandedContentHeight = wayPoint.Count == 0? StyleSheet.AccordionNodeContent_LabelHeight : wayPoint.Count * StyleSheet.AccordionNodeContent_LabelHeight
                    },
                    fishListView);

                byWayPointLayout.Children.Add(acWayPointNode);
            }

            foreach (var fishSpeciesCaught in _viewModel.FishCaughtBySpecies)
            {
                var contentListView = CreateFishListView(fishSpeciesCaught);
                var acSpeciesNode = AccordionFactory.CreateNewNode(
                    new AcNodeConfiguration()
                    {
                        HeaderText = fishSpeciesCaught.SpeciesGroupName,
                        HeaderBackGroundColor = StyleSheet.Button_BackColor,
                        HeaderTextColor = StyleSheet.Button_TextColor,
                        FontSize = StyleSheet.AccordionNode_HeaderFontSize,
                        HeaderFontAttributes = StyleSheet.AccordNode_HeaderFontAttributes,
                        ExpandedContentHeight  = fishSpeciesCaught.Count == 0 ? StyleSheet.AccordionNodeContent_LabelHeight : fishSpeciesCaught.Count * StyleSheet.AccordionNodeContent_LabelHeight
                    }, contentListView);

                bySpeciesLayout.Children.Add(acSpeciesNode);
            }

        }

        private View CreateFishListView(FishOnGroupBySpeciesViewModel speciesFish)
        {
            ListViewAlternatingRowProcessor lViewHelper = new ListViewAlternatingRowProcessor(StyleSheet.ListView_EvenRowBackColor,
                                                                                              StyleSheet.ListView_OddRowBackColor,
                                                                                              StyleSheet.ListView_SelectedRowBackColor);

            var dataTemplate = new DataTemplate(() =>
            {
                Label wayPointLabel = new Label() { FontSize = 18 };
                wayPointLabel.SetBinding(Label.TextProperty, "WayPoint.Name");

                Label dateCaughtLabel = new Label() { HorizontalOptions = LayoutOptions.EndAndExpand };
                dateCaughtLabel.SetBinding(Label.TextProperty, "DateTimeCaughString");

               
                return new ViewCell()
                {
                    View = new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        Padding = new Thickness(5, 5),
                        Children = { wayPointLabel, dateCaughtLabel },
                        
                    }
                };
            });

             ListView result = new ListView()
            {
                BindingContext = this.BindingContext,
                ItemsSource = speciesFish,
                ItemTemplate = dataTemplate
            };

            result.ItemSelected += async (sender, args) =>
            {
                await _viewModel.FishTapped((Model.FishOn)args.SelectedItem);
            };

            return result;
        }

        private View CreateFishListView(FishOnGroupByWayPointViewModel wayPointFish)
        {
            ListViewAlternatingRowProcessor rowProcessor = new ListViewAlternatingRowProcessor(StyleSheet.ListView_EvenRowBackColor, StyleSheet.ListView_OddRowBackColor, StyleSheet.ListView_SelectedRowBackColor);

            var dataTemplate = new DataTemplate(() =>
            {
                Label speciesLabel = new Label() { FontSize = 18 };
                speciesLabel.SetBinding(Label.TextProperty, "Species.Name");

                Label dateCaughtLabel = new Label() { HorizontalOptions = LayoutOptions.EndAndExpand };
                dateCaughtLabel.SetBinding(Label.TextProperty, "DateTimeCaughString");


                var viewCell = new ViewCell()
                {
                    View = new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        Padding = new Thickness(5, 5),
                        Children = {speciesLabel, dateCaughtLabel}
                    }
                };

                viewCell.Appearing += (object sender, EventArgs e) =>
                {
                    rowProcessor.SetBackColor(sender);
                };

                return viewCell;
            });

            ListView result = new ListView()
            {
                BindingContext = this.BindingContext,
                ItemsSource = wayPointFish,
                ItemTemplate = dataTemplate
            };
            
            
            result.ItemSelected += async (sender, args) =>
            {
                await _viewModel.FishTapped((Model.FishOn)args.SelectedItem);
            };
           
            return result;
        }

      
        protected void AddToggleButtons()
        {
            if (toggleButtonsLayout.Children.Count == 0)
            {
                var buttonWidth = ((int)Application.Current.MainPage.Width / 2) -
                                  (this.Padding.Left + this.Padding.Right);
                Button bySpeciesToggle = new Button { WidthRequest = buttonWidth, Text = "By Species" };
                bySpeciesToggle.SetBinding(Button.CommandProperty, "ToggleViews");
                bySpeciesToggle.SetBinding(Button.BackgroundColorProperty, "BySpeciesButtonColor");
                bySpeciesToggle.SetBinding(Button.IsEnabledProperty, "BySpeciesButtonEnabled");
                bySpeciesToggle.SetBinding(Button.TextColorProperty, "ButtonTextColor");

                Button byWayPointToggle = new Button { WidthRequest = buttonWidth, BackgroundColor = Color.Green, Text = "By Waypoint" };
                byWayPointToggle.SetBinding(Button.CommandProperty, "ToggleViews");
                byWayPointToggle.SetBinding(Button.BackgroundColorProperty, "ByWayPointButtonColor");
                byWayPointToggle.SetBinding(Button.TextColorProperty, "ButtonTextColor");
                byWayPointToggle.SetBinding(Button.IsEnabledProperty, "ByWayPointButtonEnabled");

                toggleButtonsLayout.Children.Add(byWayPointToggle);
                toggleButtonsLayout.Children.Add(bySpeciesToggle);
            }
        }
    }
}
