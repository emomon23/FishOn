using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FishOn.Pages_MVs;
using FishOn.Repositories;
using FishOn.Services;
using FishOnMobileApp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FishOn.ModelView
{
    public abstract class BaseModelView : INotifyPropertyChanged
    {
      
        protected bool _isBusy = false;
        protected FishOnNavigationService _navigation;
        protected IFishOnService _fishOnService;

        public event PropertyChangedEventHandler PropertyChanged;

        public BaseModelView(FishOnNavigationService navigation, IFishOnService fishOnService)
        {
            _navigation = navigation;
            _fishOnService = fishOnService;
        }
     

        //for bindings
        public Color ButtonBackColor => StyleSheet.Button_BackColor;
        public Color ButtonTextColor => StyleSheet.Button_TextColor;
        public Color AccentColor => StyleSheet.NavigationPage_BarTextColor;

        public double HalfPageWidth => Application.Current.MainPage.Width / 2;
        public double HalfPageHeight => Application.Current.MainPage.Height / 2;

        public int Default_Label_Font_Size => StyleSheet.Default_Label_Font_Size;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        protected async Task<bool> AreYouSureAsync(string prompt)
        {
            var answer =  await App.Current.MainPage.DisplayAlert("Are you sure?",
                       prompt, "Yes", "No");

            return answer;
        }

        public virtual async Task InitializeAsync() { }

        protected void OnPropertyChanged([CallerMemberName] string name="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }

    //Found this work around at: https://forums.xamarin.com/discussion/comment/105285/#Comment_105285
    //When a list view is bound to a collection and for each collection a element with a Command={Binding SomeCommand} is specified
    //The call won't call it on the parent viewmodel, it expects to find it on the context for the list item
    [ContentProperty("ElementName")]
    public class ElementSource : IMarkupExtension
    {
        public string ElementName { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            var rootProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            if (rootProvider == null)
                return null;
            var root = rootProvider.RootObject as Element;
            if (root == null)
                return null;
            return root.FindByName<Element>(ElementName);
        }
    }
}
