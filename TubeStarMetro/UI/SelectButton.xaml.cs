using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace TubeStarMetro
{
    public sealed partial class SelectButton : UserControl
    {
        public event EventHandler SelectedChanged;

        private VideoAttribute _videoAttribute;
        public VideoAttribute VideoAttribute
        {
            get { return _videoAttribute; }
            set
            {
                _videoAttribute = value;
                txtButtonText.Text = String.Format("{0}", _videoAttribute.Name);
                txtDetails.Text = _videoAttribute.Description;

                string points = String.Empty;
                for (int i = 0; i < _videoAttribute.Cost; i++)
                {
                    points += "•";
                }
                txtPoints.Text = points;
            }
        }

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                btnToggle.IsChecked = value;
                if (SelectedChanged != null)
                    SelectedChanged(this, EventArgs.Empty);
            }
        }

        public SelectButton()
        {
            InitializeComponent();
        }

        private void Toggle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Selected = !Selected;
            btnToggle.ToggleClicked();
        }

        private void Toggle_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}