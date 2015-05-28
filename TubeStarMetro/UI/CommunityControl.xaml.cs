using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace TubeStarMetro
{
    public sealed partial class CommunityControl : UserControl
    {
        [JsonObject()]
        public class FeaturedVideo
        {
            [JsonProperty("Id")]
            public int Id { get; set; }

            [JsonProperty("Featured")]
            public bool Featured { get; set; }

            [JsonProperty("YouTubeId")]
            public string YouTubeId { get; set; }
        }

        public event Action BackClicked;

        public CommunityControl()
        {
            InitializeComponent();

            videoPanel.Items.Add(new ShareVideoBlock());
            Populate();
            Translate();
        }

        private async void Populate()
        {
            var videos = await WebClientHelpers.Download<List<FeaturedVideo>>(new Uri("http://tubestar.azurewebsites.net/Featured/Fetch"));
            List<Video> tempVids = new List<Video>();
            foreach (var video in videos)
            {
                var vid = new Video()
                {
                    Name = "Featured Video",
                    YouTubeVideoId = video.YouTubeId,
                };
                tempVids.Add(vid);

                VideoBlock block = new VideoBlock(new Channel(), vid);
                block.Featured = video.Featured;
                block.HideInformation();

                videoPanel.Items.Add(block);
            }

            Parallel.ForEach(tempVids, async video =>
            {
                Uri photoUri = YouTubeAPI.GetPhotoUri(new YouTubeSearchId() { VideoId = video.YouTubeVideoId });
                var stream = await WebClientHelpers.DownloadImage(photoUri);
                if (stream != null)
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        video.ExternalSetImageBytes(StreamHelpers.StreamToBytes(stream));
                    });
                }
            });
        }

        private void Translate()
        {
        }

        private async void VideoPanel_ItemClick(object sender, ItemClickEventArgs e)
        {
            var block = e.ClickedItem as VideoBlock;
            if (block != null)
            {
                ViewVideoDialog dlg = new ViewVideoDialog(block.Video);
                await dlg.ShowAsync();
            }

            var shareBlock = e.ClickedItem as ShareVideoBlock;
            if (shareBlock != null)
            {
                await Launcher.LaunchUriAsync(new Uri("http://tubestar.azurewebsites.net/Featured"));
            }
        }

        private void Back_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (BackClicked != null)
                BackClicked();
        }
    }
}