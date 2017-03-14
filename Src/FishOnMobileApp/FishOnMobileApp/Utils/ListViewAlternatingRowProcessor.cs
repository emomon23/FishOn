using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FishOn.Utils
{
    public class ListViewAlternatingRowProcessor
    {
        private bool _isEvenRow;
        private Color _evenRowColor;
        private Color _oddRowColor;
        private Color _tappedColor;

        private ViewCell _previouslyTappedCell = null;
        private Color? _previouslyTappedCellNaturalBackColor;

        public ListViewAlternatingRowProcessor(Color evenBackColor, Color oddBackColor, Color tappedColor)
        {
            _evenRowColor = evenBackColor;
            _oddRowColor = oddBackColor;
            _tappedColor = tappedColor;
        }

        public void SetBackColor(object viewCellSender)
        {
            var viewCell = (ViewCell) viewCellSender;

            Color bg = _oddRowColor;

            viewCell.Tapped += ViewCell_Tapped;

            if (_isEvenRow)
            {
                bg = _evenRowColor;
            }

            _isEvenRow = !_isEvenRow;

            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = bg;

            }
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            var viewCell = (ViewCell) sender;

            if (_previouslyTappedCellNaturalBackColor.HasValue)
            {
                _previouslyTappedCell.View.BackgroundColor = _previouslyTappedCellNaturalBackColor.Value;
            }

            if (viewCell.View != null)
            {
                _previouslyTappedCellNaturalBackColor = viewCell.View.BackgroundColor;
                viewCell.View.BackgroundColor = _tappedColor;

                _previouslyTappedCell = viewCell;
            }
        }
    }
}
