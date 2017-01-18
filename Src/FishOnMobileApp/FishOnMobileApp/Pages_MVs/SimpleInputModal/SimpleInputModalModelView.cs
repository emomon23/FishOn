using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FishOn.Utils;
using Xamarin.Forms;

namespace FishOn.ModelView
{
    public class SimpleInputModalModelView :BaseModelView
    {
        public delegate Task ModalClosedAsyncCallBackDelegate(bool cancelClicked, string valueProvided, bool deleteClicked);

        private ModalClosedAsyncCallBackDelegate _modalClosedAsyncCallBack;
        private string _okButtonText = "Ok";
        private string _cancelButtonText = "Cancel";
        private string _captionText;
        private string _inputBoxValue;
        private bool _showDelete = false;

        public SimpleInputModalModelView(INavigation navigation, string captionText, string okButtonText = "Ok", string cancelButtonText = "Cancel", string defaultValue=null, bool showDeleteButton = false) : base(navigation)
        {
            CaptionText = captionText;
            OkButtonText = okButtonText;
            CancelButtonText = cancelButtonText;
            ShowDeleteButton = showDeleteButton;

            if (defaultValue.IsNotNullOrEmpty())
            {
                InputBoxValue = defaultValue;
            }
        }

        public bool ShowDeleteButton
        {
            get
            {
                return _showDelete;
            }
            private set
            {
                _showDelete = value;
                OnPropertyChanged();
            }
        }
        public async Task DisplayModalAsync(ModalClosedAsyncCallBackDelegate callBack)
        {
            _modalClosedAsyncCallBack = callBack;
            var deleteButtonText = _showDelete? "Delete" : null;

            var page = new SimpleInputModal();
            page.BindingContext = this;


            await _navigation.PushModalAsync(page);
        }

        public string CaptionText
        {
            get
            {
                return _captionText;
            }
            private set
            {
                _captionText = value;
                OnPropertyChanged();
            }
        }

        public string OkButtonText
        {
            get { return _okButtonText; }
            private set
            {
                _okButtonText = value;
                OnPropertyChanged();
            }
        }

        public string CancelButtonText
        {
            get { return _cancelButtonText; }
            set
            {
                _cancelButtonText = value;
                OnPropertyChanged();
            }
        }

        public string InputBoxValue
        {
            get
            {
                return _inputBoxValue;
            }
            set
            {
                _inputBoxValue = value;
                OnPropertyChanged();
            }
        }

        public Command OkClickCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var copyValue = InputBoxValue.Substring(0);
                    await _navigation.PopModalAsync();
                    _modalClosedAsyncCallBack?.Invoke(false, copyValue, false);
                });
            }
        }

        public Command CancelClickCommand
        {
            get
            {
                return new Command(async() =>
                {
                    await _navigation.PopModalAsync();
                    _modalClosedAsyncCallBack?.Invoke(true, "", false);
                });
            }
        }

        public Command DeleteClickCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await _navigation.PopModalAsync();
                    _modalClosedAsyncCallBack?.Invoke(true, "", true);
                });
            }
        }
    }
}
