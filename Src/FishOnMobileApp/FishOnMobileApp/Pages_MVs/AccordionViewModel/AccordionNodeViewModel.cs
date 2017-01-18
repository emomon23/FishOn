using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace FishOn.Pages_MVs.AccordionViewModel
{
    public class AccordionNodeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Private Properties 

        private bool _isExpanded = false;
        private string _headerText;
       
        //What the height should be when expanded
        private int _expandedContentHeight;

        //What's the current height
        private int _currentContentHeight = 0;

        //These Could Be Images instead of text
        //What should display when the nodes is expanded
        private string _iconExpandButtonText;

        //What should display when the nodes is contracted
        private string _iconContractButtonText;

        //What is currrently displaying to the user
        private string _currentIconButtonText = "+";

        private bool _subContentIsVisible = false;

        #endregion

        public AccordionNodeViewModel(string headerText, int expandedContentHeight, Color headerBackgroundColor, Color headerTextColor, Color? lineColor = null, string iconExpandText = "+", string icondeContractText = "-", bool expandInitially = false)
        {

            HeaderText = headerText;
            _expandedContentHeight = expandedContentHeight;
            _iconExpandButtonText = iconExpandText;
            _iconContractButtonText = icondeContractText;
            HeaderBackGroundColor = headerBackgroundColor;
            HeaderTextColor = headerTextColor;
            LineColor = lineColor ?? headerTextColor;

            if (expandInitially)
            {
                ExpandContractAccordion.Execute(null);
            }
        }

        public int ContentHeight
        {
            get { return _currentContentHeight; }
            private set
            {
                _currentContentHeight = value;
                OnPropertyChanged();
            }
        }


        public string HeaderText
        {
            get
            {
                return _headerText;
            }
            private set
            {
                _headerText = value;
                OnPropertyChanged();
            }
        }

        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            private set
            {
                _isExpanded = value;
                OnPropertyChanged();
            }
        }

        public string IconText
        {
            get { return _currentIconButtonText; }
            private set
            {
                _currentIconButtonText = value;
                OnPropertyChanged();
            }
        }

        public bool SubContentIsVisible
        {
            get
            {
                return _subContentIsVisible;
            }
            private set
            {
                _subContentIsVisible = value;
                OnPropertyChanged();
            }
        }

        public void ShowSubContent()
        {
            SubContentIsVisible = true;
            ContentHeight = _expandedContentHeight;
        }

        public void HideSubContent(int? reduceSize = null)
        {
            SubContentIsVisible = false;

            if (reduceSize.HasValue)
            {
                ContentHeight = reduceSize.Value;
            }
        }

        public Color HeaderBackGroundColor { get; private set; }

        public Color HeaderTextColor { get; private set; }

        public Color LineColor { get; private set; }

        
        public ICommand ExpandContractAccordion
        {
            get
            {
                return new Command(() =>
                {
                  
                    if (IsExpanded)
                    {
                        IsExpanded = false;
                        IconText = _iconExpandButtonText;
                        ContentHeight = 0;
                    }
                    else
                    {
                        IsExpanded = true;
                        IconText = _iconContractButtonText;
                        ContentHeight = _expandedContentHeight;
                    }
                });
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
