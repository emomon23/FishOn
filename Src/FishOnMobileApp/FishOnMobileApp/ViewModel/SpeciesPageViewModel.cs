using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FishOn.ViewModel
{
    public class SpeciesPageViewModel : BaseViewModel
    {
        public SpeciesPageViewModel(INavigation navigation) : base(navigation) { }

        public ICommand CancelCommand
        {
            get
            {
                return new Command(async () => await Navigate_BackToLandingPage() );
            }
        }

    }
}
