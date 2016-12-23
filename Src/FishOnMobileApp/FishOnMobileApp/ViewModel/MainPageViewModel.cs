using System.Windows.Input;
using Xamarin.Forms;

namespace FishOn.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        private bool _isBusy = false;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                base.OnPropertyChanged();
            }
        }
      
        public ICommand FishOnCommand { get { return new Command(() =>
                {
                    IsBusy = true;
                });
            }
        }

        //Here is another way to do a ICommand
        //Note you could also create a private ICommand _findMySpotCommand and set it on the constructor
        //and simply return it
        public ICommand FindMySpotCommand {
            get { return new Command(FindMySpot); }
        }

        private void FindMySpot()
        {
            IsBusy = false;
        }
    }
}
