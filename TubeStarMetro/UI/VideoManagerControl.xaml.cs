using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
namespace TubeStarMetro
{
    public sealed partial class VideoManagerControl : UserControl
    {
        private Channel _currentChannel;

        public VideoManagerControl()
        {
            InitializeComponent();
            Translate();
            _currentChannel = Channel.UnreleasedVideos;

            btnAddChannel.HandPointerCursor();
            btnStats.HandPointerCursor();
        }

        private void Translate()
        {
            txtAddChannel.Text = EnglishStrings.AddChannel.Translate();
            ToolTipService.SetToolTip(btnStats, new ToolTip() { Content = EnglishStrings.Stats.Translate() });
        }

        private async void AddChannel_Click(object sender, RoutedEventArgs e)
        {
            AddChannelDialog channelDialog = new AddChannelDialog();
            if (await channelDialog.ShowAsync() == true)
            {
                if (channelDialog.Channel != null)
                {
                    Player.Current.Channels.Add(channelDialog.Channel);
                    Update();
                }
            }
        }

        public void Update()
        {
            //Update unreleased videos channel...
            Channel.UnreleasedVideos.Videos.Clear();
            foreach (var video in Player.Current.Videos)
            {
                if (video.ReadyForRelease())
                {
                    Channel.UnreleasedVideos.Videos.Add(video);
                }
            }

            channelPanel.Items.Clear();
            foreach (var channel in Player.Current.Channels.Where(c => !c.IsSuspended))
            {
                RenderChannel(channel);
            }

            foreach (var channel in Player.Current.Channels.Where(c => c.IsSuspended))
            {
                RenderChannel(channel);
            }

            ShowChannel(_currentChannel);
        }

        private void RenderChannel(Channel channel)
        {
            var channelBlock = new ChannelBlock(channel, channel == _currentChannel);
            channelBlock.EditClick += async (s, e) =>
            {
                AddChannelDialog channelDialog = new AddChannelDialog(channelBlock.Channel);
                if (await channelDialog.ShowAsync() == true)
                {
                    channelBlock.Channel.Name = channelDialog.Channel.Name;
                    channelBlock.Channel.Advertising = channelDialog.Channel.Advertising;
                    Update();
                }
            };
            channelPanel.Items.Add(channelBlock);
        }

        private void ShowChannel(Channel channel)
        {
            channelSummaryTextBlock.Text = (channel == Channel.UnreleasedVideos) ?
                EnglishStrings.NotApplicable.Translate() :
                channel.IsSuspended ? EnglishStrings.Suspended.Translate().ToUpper() : String.Format("{0}: {1}  {2}: {3}", EnglishStrings.Subscribers.Translate(), (channel.Subscribers.Count * InternetGraph.Multiplier).ToNumberString(), EnglishStrings.ChannelIncome.Translate(), channel.Income.ToCurrencyString());
            btnStats.Visibility = (channel == Channel.UnreleasedVideos) ? Visibility.Collapsed : Visibility.Visible;

            videoPanel.Items.Clear();

            foreach (var video in channel.Videos)
            {
                var videoBlock = new VideoBlock(channel, video);
                videoBlock.UploadClick += (s, e) =>
                {
                    UploadVideo(videoBlock.Video);
                };
                videoBlock.DeleteClick += (s, e) =>
                {
                    CustomMessageBox.ShowDialog(EnglishStrings.Delete.Translate(), EnglishStrings.DeleteVideo.Translate(), MessagePicture.Question, (result) =>
                    {
                        if (result == true)
                        {
                            Player.Current.Videos.Remove(videoBlock.Video);
                            channel.Videos.Remove(videoBlock.Video);

                            Update();
                        }
                    });
                };

                videoBlock.LawyerClick += (s, e) =>
                {
                    if (_currentChannel.IsSuspended)
                    {
                        CustomMessageBox.ShowDialog(EnglishStrings.LawyerStoreItem.Translate(), String.Format(EnglishStrings.RemoveChannelSuspension.Translate(), StoreItems.Current.Lawyer.AdditionalCost.ToCurrencyString()), MessagePicture.Legal, (result) =>
                        {
                            if (result == true)
                            {
                                _currentChannel.IsSuspended = false;
                                foreach (var v in _currentChannel.Videos)
                                {
                                    v.IsSuspended = false;
                                }
                                Player.Current.Money -= StoreItems.Current.Lawyer.AdditionalCost;
                                Update();
                            }
                        });
                    }
                    else if (videoBlock.Video.IsSuspended)
                    {
                        CustomMessageBox.ShowDialog(EnglishStrings.LawyerStoreItem.Translate(), String.Format(EnglishStrings.RemoveVideoSuspension.Translate(), StoreItems.Current.Lawyer.AdditionalCost.ToCurrencyString()), MessagePicture.Legal, (result) =>
                        {
                            if (result == true)
                            {
                                videoBlock.Video.IsSuspended = false;
                                Player.Current.Money -= StoreItems.Current.Lawyer.AdditionalCost;
                                Update();
                            }
                        });
                    }
                };
                videoPanel.Items.Insert(0, videoBlock);
            }
        }

        private async void UploadVideo(Video video)
        {
            if (Player.Current.Channels.Count == 1)
            {
                CustomMessageBox.ShowDialog(EnglishStrings.WhereTo.Translate(), EnglishStrings.ChannelNeeded.Translate(), MessagePicture.Puzzle);
            }
            else
            {
                UploadVideoDialog uploadDialog = new UploadVideoDialog(video);

                if (await uploadDialog.ShowAsync() == true)
                {
                    if (uploadDialog.Channel != null)
                    {
                        //Generate comments
                        CommentGenerator.GenerateRealComments(uploadDialog.Video);

                        //Subscriber views
                        VideoViewer.SubscriberView(uploadDialog.Channel, uploadDialog.Video);

                        video.GenerateQuality();

                        //Consultant fee
                        if (StoreItems.Current.Consultant.Purchased)
                        {
                            video.OnceOffCost += StoreItems.Current.Consultant.AdditionalCost;
                        }

                        //Special attribute - CopyCat
                        if (uploadDialog.Video.Attributes.Contains(VideoAttributes.CopyCat))
                        {
                            VideoViewer.FreeViews(uploadDialog.Video, VideoAttributes.CopyCat.FreeViews);
                        }

                        //Special attribute - FanBoyBait
                        if (video.Quality >= 75 && uploadDialog.Video.Attributes.Contains(VideoAttributes.FanBoyBait))
                        {
                            foreach (int userId in uploadDialog.Channel.Subscribers)
                            {
                                Player.Current.Internet.GetUser(userId).FanBoyChannels.Add(uploadDialog.Channel.Id);
                            }
                        }

                        uploadDialog.Channel.Videos.Add(uploadDialog.Video);
                        Player.Current.Videos.Remove(uploadDialog.Video);
                        Update();
                    }
                }
            }
        }

        private async void btnStats_Click(object sender, RoutedEventArgs e)
        {
            ChannelStatsDialog dialog = new ChannelStatsDialog(_currentChannel);
            await dialog.ShowAsync();
        }

        private void ChannelPanel_ItemClick(object sender, ItemClickEventArgs e)
        {
            var channelBlock = e.ClickedItem as ChannelBlock;
            if (channelBlock != null)
            {
                foreach (ChannelBlock block in channelPanel.Items)
                {
                    block.Selected = false;
                }

                channelBlock.Selected = true;
                _currentChannel = channelBlock.Channel;
                ShowChannel(channelBlock.Channel);
            }
        }

        private async void VideoPanel_ItemClick(object sender, ItemClickEventArgs e)
        {
            var block = e.ClickedItem as VideoBlock;
            if (block != null)
            {
                await block.OnClick();
            }
        }

        private async void Bin_Drop(object sender, DragEventArgs e)
        {
            string data = await e.Data.GetView().GetTextAsync();

            if (_currentChannel == Channel.UnreleasedVideos)
            {
                foreach (var video in Player.Current.Videos)
                {
                    if (video.Id.ToString() == data)
                    {
                        Player.Current.Videos.Remove(video);
                        Update();
                        break;
                    }
                }
            }
            else
            {
                foreach (var vid in _currentChannel.Videos)
                {
                    if (vid.Id.ToString() == data)
                    {
                        _currentChannel.Videos.Remove(vid);
                        Update();
                        break;
                    }
                }
            }
            binGrid.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void VideoPanel_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            var block = e.Items[0] as VideoBlock;
            if (block != null)
            {
                e.Data.SetText(block.Video.Id.ToString());
            }
        }

        private void Bin_DragEnter(object sender, DragEventArgs e)
        {
            binGrid.Background = new SolidColorBrush(Colors.Silver);
        }

        private void Bin_DragLeave(object sender, DragEventArgs e)
        {
            binGrid.Background = new SolidColorBrush(Colors.Transparent);
        }
    }
}