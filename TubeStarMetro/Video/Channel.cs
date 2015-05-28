using System;
using System.Collections.Generic;
using System.Linq;

namespace TubeStarMetro
{
    public class Channel : UniqueObject
    {
        public string Name { get; set; }

        private AdvertisingStrategy _advertising;
        public AdvertisingStrategy Advertising 
        {
            get { return _advertising; }
            set 
            {
                _advertising = value;
                _incomePerView = null;
            }
        }

        private double? _incomePerView;
        public double IncomePerView
        {
            get 
            {
                if (_incomePerView == null)
                {
                    _incomePerView = Advertising.GetAttribute<AdvertistingIncomeAttribute>().IncomePerView;
                }
                return _incomePerView.Value;
            }
        }

        public HashSet<Video> Videos { get; set; }
        public HashSet<int> Subscribers { get; set; }

        public double Income { get; set; }

        public bool IsSuspended { get; set; }

        //Stats
        public List<double> IncomeOverTime { get; set; }
        public List<double> ExpensesOverTime { get; set; }
        public List<double> SubscribersOverTime { get; set; }

        public Channel()
            : base()
        {
            Videos = new HashSet<Video>();
            Subscribers = new HashSet<int>();
            Advertising = AdvertisingStrategy.Normal;
            IncomeOverTime = new List<double>();
            ExpensesOverTime = new List<double>();
            SubscribersOverTime = new List<double>();
        }

        private static Channel _unreleasedVideos;
        public static Channel UnreleasedVideos
        {
            get
            {
                if (_unreleasedVideos == null)
                    _unreleasedVideos = new Channel() { Name = EnglishStrings.UnreleasedVideos.Translate() };
                return _unreleasedVideos;
            }
            set { _unreleasedVideos = value; }
        }

        public void Merge(Channel other)
        {
            Name = other.Name;
            Advertising = other.Advertising;
            Videos = new HashSet<Video>(other.Videos);
            Subscribers = new HashSet<int>(other.Subscribers);
            Income = other.Income;
            IsSuspended = other.IsSuspended;

            IncomeOverTime = new List<double>(other.IncomeOverTime);
            ExpensesOverTime = new List<double>(other.ExpensesOverTime);
            SubscribersOverTime = new List<double>(other.SubscribersOverTime);
        }
    }
}