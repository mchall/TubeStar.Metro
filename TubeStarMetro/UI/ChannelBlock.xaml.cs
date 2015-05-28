using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace TubeStarMetro
{
    public sealed partial class ChannelBlock : UserControl
    {
        public event EventHandler EditClick;

        private Channel _channel;
        public Channel Channel
        {
            get { return _channel; }
            private set
            {
                _channel = value;
                if (_channel != null)
                {
                    UpdateText();
                    if (_channel == Channel.UnreleasedVideos)
                    {
                        btnEdit.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                rootGrid.Background = new SolidColorBrush(_selected ? Colors.Crimson : Colors.Silver);
            }
        }

        public ChannelBlock(Channel channel, bool selected)
        {
            InitializeComponent();
            Selected = selected;
            Channel = channel;
            Translate();

            this.HandPointerCursor();
            btnEdit.HandPointerCursor();
        }

        private void Translate()
        {
            txtEdit.Text = EnglishStrings.Edit.Translate();
        }

        public void UpdateText()
        {
            if (_channel != null)
            {
                channelTextBlock.Text = _channel.IsSuspended ? EnglishStrings.Suspended.Translate().ToUpper() : _channel.Name;
                txtVideoCount.Text = _channel.IsSuspended ? "" : String.Format("{0}: {1}", EnglishStrings.Videos.Translate(), _channel.Videos.Count);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditClick != null)
                EditClick(this, EventArgs.Empty);
        }

        private void UserControl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!_selected)
            {
                rootGrid.Background = new SolidColorBrush(Colors.DarkGray);
            }            
        }

        private void UserControl_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!_selected)
            {
                rootGrid.Background = new SolidColorBrush(Colors.Silver);
            }  
        }
    }
}