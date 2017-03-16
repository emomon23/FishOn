using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FishOn.Pages_MVs;
using Xamarin.Forms;

namespace FishOn.Utils
{
    public class ImgBtnGenerator
    {
        private Color _backColor;
        private Color _textColor;

        private static ImgBtnGenerator _buttonGeneraotr;

        private ImgBtnGenerator()
        {
            _backColor = StyleSheet.NavigationPage_BarBackgroundColor;
            _textColor = StyleSheet.NavigationPage_BarTextColor;
        }

        public View CreateButton(ICommand command, double width, double height, string text, string image = null,
            double? fontSize = null, Color? backColorOverride = null)
        {
            var result = CreateButtonView(width, height, text, image, fontSize, backColorOverride);
            AddTapGestureToView(result, command, text);

            return result;
        }

        public View CreateButton(Func<Task> asycFunction, double width, double height, string text, string image = null,
            double? fontSize = null, Color? backColorOverride = null)
        {
            var result = CreateButtonView(width, height, text, image, fontSize, backColorOverride);
            AddTapGestureToView(result, asycFunction, text);

            return result;
        }

        private View CreateButtonView(double width, double height, string text, string image = null,
            double? fontSize = null, Color? backColorOverride = null)
        { 

            backColorOverride = backColorOverride.HasValue ? backColorOverride : _backColor;

            StackLayout layout = new StackLayout()
            {
                Margin = new Thickness(5,5,5,5),
                Padding = new Thickness(8,8,8,8),
                BackgroundColor = backColorOverride.Value,
                WidthRequest = width,
                HeightRequest = height,
                Orientation = StackOrientation.Horizontal,
                AutomationId = $"imgBtn{text}"  //Used to prevent duplicates
            };

            if (text.IsNullOrEmpty())
            {
                //No text, just an image, center in on the 'button'
                layout.VerticalOptions = LayoutOptions.CenterAndExpand;
                layout.HorizontalOptions = LayoutOptions.CenterAndExpand;
                layout.Padding = new Thickness(2,2,2,2);
            }

            if (image.IsNotNullOrEmpty())
            {
                Image img = new Image()
                {
                    Source = image,
                    WidthRequest = text.IsNotNullOrEmpty()? width / 5 : width,
                    HeightRequest = text.IsNotNullOrEmpty()? height / 1.8 : height,
                    BackgroundColor = Color.Transparent
                };

                layout.Children.Add(img);
            }

            if (text.IsNotNullOrEmpty())
            {
                Label lbl = new Label()
                {
                    TextColor = _textColor,
                    Text = $" {text.ToUpper()}",
                    FontSize = fontSize ?? height / 2.2,
                    BackgroundColor = backColorOverride.Value,
                    HeightRequest = height - 10,
                    VerticalTextAlignment = TextAlignment.Center
                };

                layout.Children.Add(lbl);
            }
          
            return layout;
        }

        public static void ConvertToButton(View view, ICommand command)
        {
            AddTapGestureToView(view, command, null);
        }

        private static void AddTapGestureToView(View view, ICommand command, object objText)
        {
            TapGestureRecognizer tap = new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    await view.ScaleTo(.95, 200, Easing.BounceIn);
                    view.ScaleTo(1, 200);
                    command.Execute(objText);
                })
            };

            view.GestureRecognizers.Add(tap);
        }

        private static void AddTapGestureToView(View view, Func<Task> asyncFunction, object objText)
        {
            TapGestureRecognizer tap = new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    await view.ScaleTo(.95, 200, Easing.BounceIn);
                    view.ScaleTo(1, 200);
                    await asyncFunction();
                })
            };

            view.GestureRecognizers.Add(tap);
        }

        public static void AddButton(StackLayout container, ICommand command, double width,
                                        double height, string text = null, string image = null, double? fontSize=null, Color ? backColor=null)
        {
            //We don't add duplicates to the stacklayout
            if (container.Children.All(c => c.AutomationId != $"imgBtn{text}"))
            {
                var bg = ImgBtnGenerator.GetInstance();
                var button = bg.CreateButton(command, width, height, text, image, fontSize, backColor);
                container.Children.Add(button);
            }
        }

        public static void AddButton(StackLayout container, Func<Task> asyncFunction, double width,
                                       double height, string text = null, string image = null, double? fontSize = null, Color? backColor = null)
        {
            //We don't add duplicates to the stacklayout
            if (container.Children.All(c => c.AutomationId != $"imgBtn{text}"))
            {
                var bg = ImgBtnGenerator.GetInstance();
                var button = bg.CreateButton(asyncFunction, width, height, text, image, fontSize, backColor);
                container.Children.Add(button);
            }
        }


        public static ImgBtnGenerator GetInstance()
        {
            if (_buttonGeneraotr == null)
            {
                _buttonGeneraotr = new ImgBtnGenerator();
            }

            return _buttonGeneraotr;
        }

        public static ImgBtnGenerator GetInstance(Color backColor, Color texColor)
        {
            var result = GetInstance();
            result._backColor = backColor;
            result._textColor = texColor;

            return result;
        }


    }
}
