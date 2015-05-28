using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace TubeStarMetro
{
    public sealed partial class UploadVideoDialog : UserControl, IPopupControl
    {
        private PopupHelper _parent;

        public Channel Channel { get; private set; }
        public Video Video { get; private set; }

        public UploadVideoDialog(Video video)
        {
            InitializeComponent();
            Translate();

            Video = video;

            var data = GetData();
            cmbChannel.ItemsSource = data;
            cmbChannel.DisplayMemberPath = "Value";
            cmbChannel.SelectedValuePath = "Key";

            if (data.Count == 1)
            {
                cmbChannel.SelectedValue = data.First().Key;
                cmbChannel.IsEnabled = false;
            }
            else if (Settings.LastChannel != null && !Settings.LastChannel.IsSuspended)
            {
                cmbChannel.SelectedValue = Settings.LastChannel;
            }

            if (Video.ImageBytes != null)
            {
                imgVideo.SetImageFromByteArray(Video.ImageBytes);
            }

            Video.OnImageFetched += () =>
            {
                progImageFetch.Visibility = Visibility.Collapsed;
                btnFetchNewImage.IsEnabled = true;
                imgVideo.SetImageFromByteArray(Video.ImageBytes);
            };

            btnCancel.HandPointerCursor();
            btnOk.HandPointerCursor();
            btnFetchNewImage.HandPointerCursor();
            imgYT.HandPointerCursor();
            this.HandleDialogKeyboard(OKButton_Click, CancelButton_Click);
        }

        public void SetParent(PopupHelper parent)
        {
            _parent = parent;
        }

        public void OnFocus()
        {
        }

        private void Translate()
        {
            //Caption = EnglishStrings.UploadVideo.Translate();
            btnFetchNewImage.Content = EnglishStrings.FetchNewImage.Translate();
            lblBuyViews.Text = EnglishStrings.BuyViews.Translate() + ":";
            lblChannel.Text = EnglishStrings.Channel.Translate() + ":";
            lblImage.Text = EnglishStrings.Image.Translate() + ":";
            txtNone.Text = EnglishStrings.None.Translate();
            txt100.Text = String.Format("1000 {0} ({1}: $100)", EnglishStrings.Views.Translate(), EnglishStrings.Cost.Translate());
            txt200.Text = String.Format("2000 {0} ({1}: $200)", EnglishStrings.Views.Translate(), EnglishStrings.Cost.Translate());
            txt2c.Text = String.Format("1000 {0} ({1}: $0.02 {2})", EnglishStrings.Views.Translate(), EnglishStrings.Cost.Translate(), EnglishStrings.PerView.Translate());
            txt4c.Text = String.Format("2000 {0} ({1}: $0.04 {2})", EnglishStrings.Views.Translate(), EnglishStrings.Cost.Translate(), EnglishStrings.PerView.Translate());
            btnOk.Content = EnglishStrings.Ok.Translate();
            btnCancel.Content = EnglishStrings.Cancel.Translate();
        }

        private Dictionary<Channel, string> GetData()
        {
            Dictionary<Channel, string> data = new Dictionary<Channel, string>();
            foreach (var channel in Player.Current.Channels)
            {
                if (channel != Channel.UnreleasedVideos && !channel.IsSuspended)
                    data[channel] = channel.Name;
            }
            return data;
        }

        private void FetchImage_Click(object sender, RoutedEventArgs e)
        {
            progImageFetch.Visibility = Visibility.Visible;
            btnFetchNewImage.IsEnabled = false;
            Video.FetchRandomImage(false);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (cmbChannel.SelectedValue == null)
            {
                CustomMessageBox.ShowDialog(EnglishStrings.MissingValueHeader.Translate(), EnglishStrings.MissingChannel.Translate(), MessagePicture.Puzzle);
                return;
            }

            if (rb100.IsChecked == true)
            {
                if (Player.Current.Money - 100 < 0)
                {
                    CustomMessageBox.ShowDialog(EnglishStrings.LowCashHeader.Translate(), EnglishStrings.LowCashMessage.Translate(), MessagePicture.Money);
                    return;
                }
                Video.OnceOffCost += 100;
                VideoViewer.BuyViews(Video, 1000 / InternetGraph.Multiplier);
            }
            else if (rb200.IsChecked == true)
            {
                if (Player.Current.Money - 200 < 0)
                {
                    CustomMessageBox.ShowDialog(EnglishStrings.LowCashHeader.Translate(), EnglishStrings.LowCashMessage.Translate(), MessagePicture.Money);
                    return;
                }
                Video.OnceOffCost += 200;
                VideoViewer.BuyViews(Video, 2000 / InternetGraph.Multiplier);
            }
            else if (rb2c.IsChecked == true)
            {
                VideoViewer.BuyViews(Video, 1000 / InternetGraph.Multiplier);
                Video.CostPerView = 0.02;
            }
            else if (rb4c.IsChecked == true)
            {
                VideoViewer.BuyViews(Video, 2000 / InternetGraph.Multiplier);
                Video.CostPerView = 0.04;
            }

            Channel = Settings.LastChannel = (Channel)cmbChannel.SelectedValue;
            _parent.Close(true);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _parent.Close(false);
        }

        private async void YT_MouseLeftButtonDown(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.youtube.com"));
        }
    }
}