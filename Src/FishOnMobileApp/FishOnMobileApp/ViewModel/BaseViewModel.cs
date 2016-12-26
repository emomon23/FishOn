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
        private bool _isBusy = false;
        public event PropertyChangedEventHandler PropertyChanged;
       
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

        /*Example
         * public bool IsActive {
         *     get {return _isActive}
         *     set { 
         *           _isActive = value;
         *           OnPropertyChanges()
         *           OnPropertyChanges(nameof(SomeCalculatedMessageThatUsesIsActive))
         *         }
         * }
         */

    }
}
