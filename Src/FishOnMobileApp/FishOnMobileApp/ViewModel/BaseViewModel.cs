using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FishOn.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private readonly INavigation _navigation;
        private bool _isBusy = false;
        public event PropertyChangedEventHandler PropertyChanged;

        public BaseViewModel(INavigation navigation)
        {
            _navigation = navigation;
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected async Task Navigate_ToSpeciesList()
        {
            var speciesPage = new SpeciesListPage();
            speciesPage.BindingContext = new SpeciesPageViewModel(speciesPage.Navigation);

            await _navigation.PushAsync(speciesPage);
        }

        protected async Task Navigate_BackToLandingPage()
        {
            await _navigation.PopToRootAsync(true);
        }

    }
}
