using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TubeStarMetro
{
    public class Player
    {
        private static Player _current;
        public static Player Current
        {
            get
            {
                if (_current == null)
                    _current = new Player();
                return _current;
            }
            set { _current = value; }
        }

        public event Action MoneyChanged;

        private double _money;
        public double Money
        {
            get { return _money; }
            set
            {
                _money = value;
                if (MoneyChanged != null)
                {
                    MoneyChanged();
                }
            }
        }

        public bool QuitJob { get; set; }
        public bool HasPromotion { get; set; }
        public bool Overtime { get; set; }

        public List<Channel> Channels { get; set; }
        public List<TaskBase> TasksInProgress { get; set; }
        public List<Video> Videos { get; set; }

        public int ShootingSkill { get; set; }
        public int PostProductionSkill { get; set; }
        public int VideoAttributePoints { get; set; }
        public bool CanViewQualityBeforeUpload { get; set; }

        public int Iterations { get; set; }

        public int CostOfLivingExtra { get; set; }

        public bool ChallengeMode { get; set; }
        public bool RobotRulers { get; set; }

        public double LoanPayOff { get; set; }

        private AsyncCacheItem<InternetGraph> _internet;
        [XmlIgnore]
        public InternetGraph Internet
        {
            get
            {
                _internet.WaitForPopulate();
                return _internet.Value;
            }
        }

        public Player()
        {
            _internet = new AsyncCacheItem<InternetGraph>(() => new InternetGraph());
            Reset();
        }

        public void Reset()
        {
            RobotRulers = false;
            Iterations = -1;
            Money = 950;
            QuitJob = false;
            ShootingSkill = 30;
            PostProductionSkill = 20;
            VideoAttributePoints = 2;
            CanViewQualityBeforeUpload = false;
            CostOfLivingExtra = 0;
            Overtime = false;
            HasPromotion = false;
            LoanPayOff = 0;

            TasksInProgress = new List<TaskBase>();
            Videos = new List<Video>();
            Channel.UnreleasedVideos = null;
            Channels = new List<Channel>() { Channel.UnreleasedVideos }; //Default channel

            Studies.Current = null;
            StoreItems.Current = null;
        }

        public void BornToRule()
        {
            Reset();
            Money = 19950;
            foreach (var study in Studies.Current.All)
            {
                study.HoursPutIn = study.HoursToComplete;
                switch (study.SkillModifierType)
                {
                    case SkillModifierType.PostProduction:
                        PostProductionSkill += study.SkillModifier;
                        break;
                    case SkillModifierType.Shooting:
                        ShootingSkill += study.SkillModifier;
                        break;
                    case SkillModifierType.VideoAttribute:
                        VideoAttributePoints += study.SkillModifier;
                        break;
                    case SkillModifierType.ViewQuality:
                        CanViewQualityBeforeUpload = true;
                        break;
                }
            }
            foreach (var item in StoreItems.Current.All)
            {
                if (item != StoreItems.Current.Loan)
                {
                    item.Purchased = true;
                }
            }
        }

        public void Merge(Player other)
        {
            RobotRulers = other.RobotRulers;
            Iterations = other.Iterations;
            Money = other.Money;
            QuitJob = other.QuitJob;
            ShootingSkill = other.ShootingSkill;
            PostProductionSkill = other.PostProductionSkill;
            VideoAttributePoints = other.VideoAttributePoints;
            CanViewQualityBeforeUpload = other.CanViewQualityBeforeUpload;
            CostOfLivingExtra = other.CostOfLivingExtra;
            Overtime = other.Overtime;
            HasPromotion = other.HasPromotion;
            LoanPayOff = other.LoanPayOff;

            TasksInProgress = new List<TaskBase>(other.TasksInProgress);
            Videos = new List<Video>(other.Videos);

            if (other.Channels.Count > 0)
            {
                Channel.UnreleasedVideos.Merge(other.Channels[0]);
                Channels = new List<Channel>() { Channel.UnreleasedVideos };

                var copy = new List<Channel>(other.Channels);
                copy.RemoveAt(0);
                Channels.AddRange(copy);
            }
            else
            {
                Channels = new List<Channel>() { Channel.UnreleasedVideos }; //Default channel
            }
        }
    }
}