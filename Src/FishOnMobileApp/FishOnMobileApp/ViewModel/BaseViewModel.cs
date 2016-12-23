using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FishOn.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
