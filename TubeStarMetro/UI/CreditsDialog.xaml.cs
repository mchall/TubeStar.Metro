using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace TubeStarMetro
{
    public sealed partial class CreditsDialog : UserControl, IPopupControl
    {
        private PopupHelper _parent;

        public CreditsDialog()
        {
            InitializeComponent();

            btnOk.HandPointerCursor();
            imgPixabay.HandPointerCursor();
            imgStockXchang.HandPointerCursor();
            imgYouTube.HandPointerCursor();
            imgStockPhotosForFree.HandPointerCursor();
            this.HandleDialogKeyboard(OKButton_Click, OKButton_Click);
        }

        public void SetParent(PopupHelper parent)
        {
            _parent = parent;
        }

        public void OnFocus()
        {
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            _parent.Close(true);
        }

        private async void Pixabay_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.pixabay.com"));
        }

        private async void StockXchange_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.sxc.hu/"));
        }

        private async void YouTube_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://www.youtube.com/"));
        }

        private async void StockPhotosForFree_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.stockphotosforfree.com/"));
        }
    }
}