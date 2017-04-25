﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using FishOn.Pages_MVs;
using FishOn.VoiceRecognitionSystem;

namespace FishOn.Utils
{
    public class FieldSetGenerator
    {
        private Dictionary<string, FieldSetViewContainer> _fieldSets = new Dictionary<string, FieldSetViewContainer>();
        private StackLayout _mainLayout;
        private ScrollView _mainScrollView;
        private VoiceToText _voiceToText;

        public FieldSetGenerator(StackLayout layout)
        {
            _mainLayout = layout;
        }

        public FieldSetGenerator(ScrollView scrollView)
        {
            _mainScrollView = scrollView;
        }

        public Color DefaultPlaceHolderColor = Color.FromHex("#CACFD2");
        public Color DefaultInputBackColor = Color.White;
        public Color DefaultLabelColor = Color.White;

        public FieldSetViewContainer Container(string id)
        {
            if (_fieldSets.ContainsKey(id))
            {
                return _fieldSets[id];
            }

            return null;
        }

        public FieldSetViewContainer CreateFieldSet(string fieldSetId, string labelText, string binding, bool includeButtonContainer = false)
        {
            return CreateFieldSet(fieldSetId, labelText, null, binding, includeButtonContainer);
        }

        public FieldSetViewContainer CreateVoiceToTextFieldSet(string fieldSetId, string labelText , string binding)
        {
            ICommand nullCommand = null;

            if (_voiceToText == null)
            {
                _voiceToText = new VoiceToText(_mainLayout);
            }

            var container = CreateFieldSet(fieldSetId, labelText, binding, includeButtonContainer: true);
            var button = ImgBtnGenerator.AddButton(container.ButtonContainer, nullCommand, StyleSheet.SmallSmall_Button_Width, StyleSheet.SmallSmall_Button_Height, image: "microphone.png");
            _voiceToText.BindToNativeSpeechRecognistion((Entry)container.InputElement, button, value =>
            {
                container.UpdateInputBackColor(Color.Black);
            });

            return container;
        }
        

        public FieldSetViewContainer CreateFieldSet(string fieldSetId, string labelText, View inputView, string currentValue = null, bool includeButtonContainer = false)
        {

            /* - Stacklayout (container)
             *    -Label
             *    -StackLayout Horizontal (input container)
             *       -Entry  (inputElement)
             *       -[StackLayout] (button container)
             *    -[ScrollView] for auto complete
             */

            if (_fieldSets.ContainsKey(fieldSetId))
            {
                throw new Exception($"a fieldset was already created with the key '{fieldSetId}', unable to create duplicate");
            }

            FieldSetViewContainer result = EvaluateInputView(labelText, currentValue, inputView, includeButtonContainer);
            CreateLabel(labelText, result);
            CreateButtonContainer(result, includeButtonContainer);

            AssembleFieldSet(result);

            _fieldSets.Add(fieldSetId, result);

            if (_mainScrollView != null)
            {
                _mainScrollView.Content = result.Container;
            }
            else if (_mainLayout != null)
            {
                _mainLayout?.Children.Add(result.Container);
            }

            return result;
        }

        public FieldSetViewContainer CreateAutoCompleteFieldSet(AutoCompleteDefinition autoCompleteDefinition)
        {
            FieldSetViewContainer result = null;

            if (autoCompleteDefinition.IncludeVoiceToTextButton)
            {
                result = CreateVoiceToTextFieldSet(autoCompleteDefinition.Identifier, autoCompleteDefinition.LabelText,
                    autoCompleteDefinition.Binding);
            }
            else
            {
                result = CreateFieldSet(autoCompleteDefinition.Identifier, autoCompleteDefinition.LabelText,
                    autoCompleteDefinition.Binding, autoCompleteDefinition.IncludeVoiceToTextButton);
            }

            result.AutoCompleteListContainer = new ScrollView();
            result.Container.Children.Add(result.AutoCompleteListContainer);

            Entry entry = (Entry) result.InputElement;

           

            int autoCompleteIndex = _fieldSets.Count -1;

            entry.TextChanged += (s, e) =>
            {
                var subData = autoCompleteDefinition.DataSource.Where(x => x.ToLower().Contains(entry.Text.ToLower()));
                result.AutoCompleteListContainer.IsVisible = entry.Text != "" && subData.Any() &&
                                                             !(subData.FirstOrDefault() == entry.Text);

                ShowHideAllOtherFieldSets(autoCompleteIndex, !result.AutoCompleteListContainer.IsVisible);

                StackLayout sl = new StackLayout() {BackgroundColor = this.DefaultInputBackColor};

                foreach (var item in subData)
                {
                    Label lbl = new Label() { Text = item, HeightRequest = entry.Height, FontSize = entry.FontSize};

                    lbl.GestureRecognizers.Add(new TapGestureRecognizer()
                    {
                        Command = new Command(() =>
                        {
                            entry.Text = lbl.Text;
                        })
                    });
                    sl.Children.Add(lbl);
                }

                result.AutoCompleteListContainer.Content = sl;
            };
            
            return result;
        }

        private void ShowHideAllOtherFieldSets(int leaveIndex, bool isVisible)
        {
            for (int i = 0; i < _fieldSets.Count; i++)
            {
                if (i != leaveIndex)
                {
                    var key = _fieldSets.Keys.ElementAt(i);
                    _fieldSets[key].Container.IsVisible = isVisible;
                }
            }
        }

        private FieldSetViewContainer EvaluateInputView(string placeHolderText, string binding, View inputView, bool includeButtonContainer = false)
        {
            FieldSetViewContainer container = new FieldSetViewContainer();

            if (inputView == null)
            {
                var entry = new Entry()
                {
                    TextColor = this.DefaultPlaceHolderColor,
                    BackgroundColor = this.DefaultInputBackColor,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                };

                entry.Focused += (s, e) =>
                {
                    if (entry.Text == placeHolderText)
                    {
                        entry.Text = "";
                        entry.TextColor = Color.Black;
                    }
                };

                entry.TextChanged += (s, e) =>
                {
                    if (entry.Text != placeHolderText)
                    {
                        entry.TextColor = Color.Black;
                    }   
                    else if (entry.Text == "")
                    {
                        entry.Text = placeHolderText;
                        entry.TextColor = this.DefaultPlaceHolderColor;
                    }
                };

                entry.SetBinding(Entry.TextProperty, binding);
                inputView = entry;
            }
            
            container.InputElement = inputView;
            return container;
        }
        
        private void CreateLabel(string labelText, FieldSetViewContainer container)
        {
            Label lbl = new Label()
            {
                Text = labelText,
                FontSize = 15,
                Margin = 1,
                TextColor = DefaultLabelColor
            };

            container.Label = lbl;
        }

        private void CreateButtonContainer(FieldSetViewContainer container, bool create)
        {
            if (create)
            {
                StackLayout btnContainerLayout = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.End
                };

                container.ButtonContainer = btnContainerLayout;
            }
        }

        private void AssembleFieldSet(FieldSetViewContainer container)
        {
            container.InputContainer = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Padding = 0,
                Spacing = 0,
                Margin = 0,
                Children = { container.InputElement}
            };

            if (container.ButtonContainer != null)
            {
                container.InputContainer.Children.Add(container.ButtonContainer);
            }

            var containerTop = _fieldSets.Count == 0 ? 5 : 10;
            var containerRight = container.ButtonContainer == null ? 10 : 0;

            container.Container = new StackLayout()
            {
                Margin = new Thickness(10, containerTop, containerRight,0),
                Children = {container.Label, container.InputContainer}
            };
        }
    }

    public class FieldSetViewContainer
    {
        public StackLayout Container { get; set; }
        public Label Label { get; set; }
        public StackLayout InputContainer { get; set; }
        public View InputElement { get; set; }
        public StackLayout ButtonContainer { get; set; }
        public View Button { get; set; }
        public ScrollView AutoCompleteListContainer { get; set; }
       
        public void UpdateInputBackColor(Color color)
        {
            ((Entry) this.InputElement).TextColor = color;
        }
    }

    public class AutoCompleteDefinition
    {
        public string Identifier { get; set; }
        public string LabelText { get; set; }
        public string Binding { get; set; }
        public bool IncludeVoiceToTextButton { get; set; }
        public List<string> DataSource { get; set; }
        public Action OnExpand { get; set; }
        public Action OnCollapse { get; set; }
        public Action OnSelected { get; set; }
    }
}
