using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FishOn.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel(INavigation navigation) : base(navigation)
        {
        }

        public ICommand FishOnCommand { get { return new Command(async () =>
                {
                    try
                    {
                        IsBusy = true;
                        await Navigate_ToSpeciesList();
                    }
                    finally
                    {
                        IsBusy = false;
                    }
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
