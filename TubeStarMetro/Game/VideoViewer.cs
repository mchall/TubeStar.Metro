using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace TubeStarMetro
{
    public class SerializableShareData
    {
        public Guid Id { get; set; }
        public HashSet<int> Data { get; set; }
    }

    public static class VideoViewer
    {
        private static ConcurrentDictionary<Guid, HashSet<int>> _videoShares;
        private static ConcurrentDictionary<Guid, HashSet<int>> _videoBoughtViews;

        public static List<SerializableShareData> GetShares()
        {
            List<SerializableShareData> data = new List<SerializableShareData>();
            foreach (var item in _videoShares)
            {
                data.Add(new SerializableShareData() { Id = item.Key, Data = item.Value });
            }
            return data;
        }

        public static void SetShares(List<SerializableShareData> data)
        {
            _videoShares.Clear();
            foreach (var item in data)
            {
                _videoShares[item.Id] = item.Data;
            }
        }

        public static List<SerializableShareData> GetBoughtViews()
        {
            List<SerializableShareData> data = new List<SerializableShareData>();
            foreach (var item in _videoBoughtViews)
            {
                data.Add(new SerializableShareData() { Id = item.Key, Data = item.Value });
            }
            return data;
        }

        public static void SetBoughtViews(List<SerializableShareData> data)
        {
            _videoBoughtViews.Clear();
            foreach (var item in data)
            {
                _videoBoughtViews[item.Id] = item.Data;
            }
        }

        private static InternetGraph Internet
        {
            get { return Player.Current.Internet; }
        }

        static VideoViewer()
        {
            Reset();
        }

        public static void Reset()
        {
            _videoShares = new ConcurrentDictionary<Guid, HashSet<int>>();
            _videoBoughtViews = new ConcurrentDictionary<Guid, HashSet<int>>();
        }

        public static void BuyViews(Video video, int number)
        {
            if (!_videoBoughtViews.ContainsKey(video.Id) || _videoBoughtViews[video.Id] == null)
            {
                _videoBoughtViews[video.Id] = new HashSet<int>();
            }

            for (int i = 0; i < number; i++)
            {
                _videoBoughtViews[video.Id].Add(Internet.RandomUserId);
            }
        }

        public static void FreeViews(Video video, int number)
        {
            Initialize(video);
            for (int i = 0; i < number; i++)
            {
                _videoShares[video.Id].Add(Internet.RandomUserId);
            }
        }

        public static void SubscriberView(Channel channel, Video video)
        {
            Initialize(video);
            foreach (var subsriber in channel.Subscribers)
            {
                _videoShares[video.Id].Add(subsriber);
            }
        }

        public static void Iteration(Channel channel, Video video, ref double dailyIncome, ref double dailyExpenses)
        {
            Initialize(video);
            video.Iterations++;

            if (channel.IsSuspended || video.IsSuspended)
            {
                _videoShares[video.Id] = new HashSet<int>();
                _videoBoughtViews[video.Id] = new HashSet<int>();
                return;
            }

            var tempShares = new HashSet<int>();
            foreach (var share in _videoShares[video.Id])
            {
                if (RandomHelpers.Chance(Player.Current.ChallengeMode ? 25 : 50))
                {
                    tempShares.Add(share);
                }
            }
            _videoShares[video.Id].RemoveWhere(i => tempShares.Contains(i));

            var payedViews = _videoBoughtViews.ContainsKey(video.Id) ? new HashSet<int>(_videoBoughtViews[video.Id]) : new HashSet<int>();
            _videoBoughtViews[video.Id] = new HashSet<int>();

            var preShareViews = video.Views;

            foreach (var userId in payedViews)
            {
                ViewVideo(channel, video, Internet.GetUser(userId), ref dailyIncome, ref dailyExpenses, true);
            }

            foreach (var userId in tempShares)
            {
                ViewVideo(channel, video, Internet.GetUser(userId), ref dailyIncome, ref dailyExpenses);
            }

            var searchEngineViews = preShareViews / 500;
            var qualityViews = Math.Max(0, ((video.Quality.Value - Math.Pow(video.Iterations, Player.Current.ChallengeMode ? 2 : 1)) * video.Quality.Value) / Math.Max(InternetGraph.Multiplier * (Player.Current.ChallengeMode ? 2 : 1), (100 - video.Quality.Value) / 2));
            var iterationViews = searchEngineViews + qualityViews;
            for (int i = 0; i < iterationViews; i++)
            {
                ViewVideo(channel, video, Internet.RandomUser, ref dailyIncome, ref dailyExpenses);
            }
        }

        private static void ViewVideo(Channel channel, Video video, InternetUser user, ref double income, ref double expenses, bool payedView = false)
        {
            CommentType? commentType = null;
            bool watchedAlready = false;
            if (video.ViewUsers.Contains(user.Id))
            {
                if (video.Attributes.Contains(VideoAttributes.SecondTime))
                    watchedAlready = true;
                else
                    return;
            }
            else
            {
                video.ViewUsers.Add(user.Id);
            }

            var numViews = payedView ? InternetGraph.Multiplier : GetViewersFromQuality(video.Quality.GetValueOrDefault(0));

            video.Views += numViews;
            income += ViewIncome(channel, video, payedView, numViews);
            expenses += ViewExpenses(video, numViews);

            if (watchedAlready)
                return;

            if (RandomHelpers.Chance(2))
                commentType = CommentType.Random;

            if (CategoryHelpers.CheckInterest(user, video))
            {
                if (RandomHelpers.Chance(video.Quality.Value))
                {
                    video.Likes += numViews;

                    if (RandomHelpers.Chance(1))
                        commentType = CommentType.Like;

                    if (GetSubscriptionChance(channel, video))
                    {
                        if (video.Attributes.Contains(VideoAttributes.Crowdfunding))
                        {
                            if (!payedView)
                                Player.Current.Money += VideoAttributes.Crowdfunding.Money;
                        }
                        else
                        {
                            channel.Subscribers.Add(user.Id);

                            if (RandomHelpers.Chance(1))
                                commentType = CommentType.Subscribe;
                        }
                    }

                    ShareVideo(video, user);
                }
                else if (RandomHelpers.RandomBool())
                {
                    video.Dislikes += numViews;

                    if (RandomHelpers.Chance(2))
                        commentType = CommentType.Dislike;

                    //Chance of unsubscribe because of dislike
                    if (CheckUnsubscriptions(channel, user))
                    {
                        if (RandomHelpers.Chance(1))
                            commentType = CommentType.UnsubscribeQuality;
                    }

                    //Share video if "So bad" attribute on video
                    if (video.Attributes.Contains(VideoAttributes.SoBad))
                    {
                        ShareVideo(video, user);
                    }
                }
            }
            else
            {
                //Chance of unsubscribe because of disinterest in category
                if (CheckUnsubscriptions(channel, user))
                {
                    if (RandomHelpers.Chance(1))
                        commentType = CommentType.UnsubscribeCategory;
                }
            }

            if (commentType.HasValue)
            {
                for (int i = 0; i < numViews; i++)
                {
                    if (RandomHelpers.Chance(100 / InternetGraph.Multiplier))
                    {
                        var comment = CommentGenerator.GenerateComment(video, commentType.Value);
                        var first = video.Comments.FirstOrDefault(c => c.Comment == comment);
                        if (first != null)
                            first.Likes += 1;
                        else
                            video.Comments.Add(new VideoComment(comment, 0));
                    }
                }
            }
        }

        private static int GetViewersFromQuality(int quality)
        {
            var viewers = 0;

            var jump = 100 / InternetGraph.Multiplier;

            var local = 0;
            while (local <= quality)
            {
                local += jump;
                viewers++;
            }
            return viewers;
        }

        private static bool CheckUnsubscriptions(Channel channel, InternetUser user)
        {
            if (user.FanBoyChannels.Contains(channel.Id))
                return false;

            if (GetUnsubscriptionChance(channel))
            {
                if (channel.Subscribers.Contains(user.Id))
                {
                    channel.Subscribers.Remove(user.Id);
                    return true;
                }
            }
            return false;
        }

        private static void ShareVideo(Video video, InternetUser user)
        {
            //Like enough to share
            int memeticMutator = video.Attributes.Contains(VideoAttributes.Memetic) ? 20 : 0;
            if (RandomHelpers.RandomInt(100 + memeticMutator) >= 60)
            {
                //Share
                foreach (int friendId in Internet.Edges[user.Id])
                {
                    var friend = Internet.GetUser(friendId);
                    if (CategoryHelpers.CheckInterest(friend, video))
                    {
                        _videoShares[video.Id].Add(friendId);
                    }
                }

                //Owner of site - share video on site
                if (RandomHelpers.Chance(25))
                {
                    int? siteId = Internet.GetSiteFromOwner(user.Id);
                    if (siteId.HasValue)
                    {
                        foreach (int subscriberId in Internet.Edges[siteId.Value])
                        {
                            if (RandomHelpers.RandomBool())
                            {
                                _videoShares[video.Id].Add(subscriberId);
                            }
                        }
                    }
                }
            }
        }

        private static bool GetSubscriptionChance(Channel channel, Video video)
        {
            int initialChance = 0;
            switch (channel.Advertising)
            {
                case (AdvertisingStrategy.Low):
                    initialChance = 90;
                    break;

                case (AdvertisingStrategy.Normal):
                    initialChance = 70;
                    break;

                case (AdvertisingStrategy.High):
                    initialChance = 50;
                    break;

                case (AdvertisingStrategy.Aggressive):
                    initialChance = 30;
                    break;

                default: throw new NotSupportedException();
            }

            if (video.Attributes.Contains(VideoAttributes.Hypnotic))
                initialChance += 30;

            return RandomHelpers.Chance(initialChance);
        }

        private static bool GetUnsubscriptionChance(Channel channel)
        {
            switch (channel.Advertising)
            {
                case (AdvertisingStrategy.Low): return RandomHelpers.Chance(10);
                case (AdvertisingStrategy.Normal): return RandomHelpers.Chance(20);
                case (AdvertisingStrategy.High): return RandomHelpers.Chance(40);
                case (AdvertisingStrategy.Aggressive): return RandomHelpers.Chance(60);
                default: throw new NotSupportedException();
            }
        }

        private static double ViewIncome(Channel channel, Video video, bool payedView, int numViews)
        {
            //Don't get base view payment for payed views
            var income = payedView ? 0 : channel.IncomePerView;
            if (Player.Current.ChallengeMode)
                income = income / 2;

            var extraIncome = Math.Sqrt(channel.Subscribers.Count / ((Player.Current.ChallengeMode ? 20000 : 1000) / InternetGraph.Multiplier)) * 0.01;
            income += extraIncome;

            if (video.Attributes.Contains(VideoAttributes.ProductPlacement))
            {
                income += 0.01; //Extra 1c
            }

            double totalIncome = income * numViews;
            channel.Income += totalIncome;
            return totalIncome;
        }

        private static double ViewExpenses(Video video, int numViews)
        {
            double totalCost = video.CostPerView * numViews;
            return totalCost;
        }

        private static void Initialize(Video video)
        {
            if (!_videoShares.ContainsKey(video.Id) || _videoShares[video.Id] == null)
            {
                _videoShares[video.Id] = new HashSet<int>();
            }
        }
    }
}