using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace TubeStarMetro
{
    public sealed partial class VideoBlock : UserControl
    {
        public event EventHandler UploadClick;
        public event EventHandler DeleteClick;
        public event EventHandler LawyerClick;

        private Channel _channel;

        public bool Featured { get; set; }

        private Video _video;
        public Video Video
        {
            get { return _video; }
            private set
            {
                _video = value;
                if (_video != null)
                {
                    if (_channel == null)
                        throw new NotSupportedException("Channel cannot be null");

                    bool isUnreleasedChannel = (_channel == Channel.UnreleasedVideos);

                    txtName.Text = _video.Name;
                    txtViews.Text = String.Format("{0} {1}", _video.Views.ToNumberString(), EnglishStrings.Views.Translate());
                    txtLikes.Text = _video.Likes.ToNumberString();
                    txtDislikes.Text = _video.Dislikes.ToNumberString();

                    panelStats.Visibility = isUnreleasedChannel ? Visibility.Collapsed : Visibility.Visible;

                    txtQuality.Text = GetQualityText(isUnreleasedChannel);

                    SuspendedAdorner.Visibility = _video.IsSuspended ? Visibility.Visible : Visibility.Collapsed;
                    txtSuspendedSub.Visibility = Visibility.Collapsed;
                    if (_video.IsSuspended && StoreItems.Current.Lawyer.Purchased)
                    {
                        txtSuspendedSub.Visibility = Visibility.Visible;
                    }

                    if (_video.IsSuspended)
                    {
                        imgVideo.Source = new BitmapImage(new Uri("ms-appx:/Assets/InternetDown.jpg"));
                        imgVideo.Stretch = Windows.UI.Xaml.Media.Stretch.Fill;
                    }
                    else
                    {
                        if (Video.ImageBytes != null)
                        {
                            imgVideo.SetImageFromByteArray(Video.ImageBytes);
                        }
                        else
                        {
                            Video.OnImageFetched += () =>
                            {
                                imgVideo.SetImageFromByteArray(Video.ImageBytes);
                            };
                        }
                    }
                }
            }
        }

        public VideoBlock(Channel channel, Video video)
        {
            InitializeComponent();
            Translate();
            _channel = channel;
            Video = video;

            imgVideo.HandPointerCursor();
        }

        public void HideInformation()
        {
            bottomGrid.Visibility = topGrid.Visibility = Visibility.Collapsed;
        }

        private void SuspendedAdorner_MouseLeftButtonDown(object sender, TappedRoutedEventArgs e)
        {
            if (LawyerClick != null && StoreItems.Current.Lawyer.Purchased && Video.IsSuspended)
                LawyerClick(this, EventArgs.Empty);
        }

        private string GetQualityText(bool isUnreleasedChannel)
        {
            return String.Format("{0}", (!isUnreleasedChannel || Player.Current.CanViewQualityBeforeUpload) ? String.Format("{0}%", _video.GenerateQuality()) : EnglishStrings.NotApplicable.Translate());
        }

        private void Translate()
        {
            txtSuspended.Text = EnglishStrings.Suspended.Translate().ToUpper();
            txtSuspendedSub.Text = EnglishStrings.SuspendedHireLawyer.Translate();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DeleteClick != null)
                DeleteClick(this, EventArgs.Empty);
        }

        public async Task OnClick()
        {
            if (!Video.IsSuspended)
                return;

            if (_channel == Channel.UnreleasedVideos)
            {
                if (UploadClick != null)
                    UploadClick(this, EventArgs.Empty);
            }
            else
            {
                ViewVideoDialog dlg = new ViewVideoDialog(Video);
                await dlg.ShowAsync();
            }
        }
    }
}