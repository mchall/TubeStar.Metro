using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Streams;

namespace TubeStarMetro
{
    public class Video : UniqueObject
    {
        public string Name { get; set; }
        public VideoCategory Category { get; set; }

        public int? ShootQuality { get; set; }
        public int? EditQuality { get; set; }
        public int? Quality { get; set; }
        public int? QualityBias { get; set; }

        public int Views { get; set; }
        public HashSet<int> ViewUsers { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public List<VideoComment> Comments { get; set; }

        public bool HasBeenEdited { get; set; }

        public int ExtraShootingHours { get; set; }
        public int ExtraEditingHours { get; set; }
        public int Cost { get; set; }

        public int Iterations { get; set; }

        public double CostPerView { get; set; }
        public double OnceOffCost { get; set; }

        public List<VideoAttribute> Attributes { get; set; }

        public string YouTubeVideoId { get; set; }
        public byte[] ImageBytes { get; set; }
        public List<string> RealComments { get; set; }

        public event Action OnImageFetched;

        public bool IsSuspended { get; set; }

        public double IterationIncome { get; set; }
        public double IterationExpenses { get; set; }

        public Video()
            : base()
        {
            ViewUsers = new HashSet<int>();
            Comments = new List<VideoComment>();
            Attributes = new List<VideoAttribute>();
            RealComments = new List<string>();
        }

        public void ExternalSetImageBytes(byte[] bytes)
        {
            ImageBytes = bytes;
            if (OnImageFetched != null)
                OnImageFetched();
        }

        public bool ReadyForRelease()
        {
            foreach (var task in Player.Current.TasksInProgress)
            {
                var editTask = task as EditVideo;
                if (editTask != null && editTask.Video == this)
                {
                    return false;
                }
            }
            return true;
        }

        public int GenerateQuality()
        {
            var quality = 0;

            if (!ShootQuality.HasValue)
            {
                ShootQuality = 0;

                int schoolSkill = 0;
                foreach (var study in Studies.Current.All)
                {
                    if (study.IsCompleted && study.SkillModifierType == SkillModifierType.Shooting)
                        schoolSkill += study.SkillModifier;
                }

                int itemSkill = 0;
                foreach (var item in StoreItems.Current.All)
                {
                    if (item.Purchased && item.SkillModifierType == SkillModifierType.Shooting)
                        itemSkill += item.SkillModifier;
                }

                ShootQuality += RandomHelpers.RandomInt(Player.Current.ShootingSkill - schoolSkill);
                ShootQuality += RandomHelpers.RandomInt(schoolSkill);
                ShootQuality += RandomHelpers.RandomInt(itemSkill);
                ShootQuality += RandomHelpers.RandomInt(ExtraShootingHours * 2);
                ShootQuality += RandomHelpers.RandomInt((Cost / 100) * 5);

                if (Attributes.Contains(VideoAttributes.LearnFromMistakes))
                {
                    var remove = RandomHelpers.RandomInt(5);
                    ShootQuality -= 5 + remove;
                    Player.Current.ShootingSkill += remove;
                }

                if (StoreItems.Current.Consultant.Purchased)
                {
                    ShootQuality += StoreItems.Current.Consultant.SkillModifier;
                }
            }

            quality += ShootQuality.Value;

            if (HasBeenEdited)
            {
                if (!EditQuality.HasValue)
                {
                    EditQuality = 0;

                    int schoolSkill = 0;
                    foreach (var study in Studies.Current.All)
                    {
                        if (study.IsCompleted && study.SkillModifierType == SkillModifierType.PostProduction)
                            schoolSkill += study.SkillModifier;
                    }

                    int itemSkill = 0;
                    foreach (var item in StoreItems.Current.All)
                    {
                        if (item.Purchased && item.SkillModifierType == SkillModifierType.PostProduction)
                            itemSkill += item.SkillModifier;
                    }

                    EditQuality += RandomHelpers.RandomInt(Player.Current.PostProductionSkill - schoolSkill);
                    EditQuality += RandomHelpers.RandomInt(schoolSkill);
                    EditQuality += RandomHelpers.RandomInt(itemSkill);
                    EditQuality += RandomHelpers.RandomInt(ExtraEditingHours * 3);

                    if (Attributes.Contains(VideoAttributes.LearnFromMistakes))
                    {
                        var remove = RandomHelpers.RandomInt(5);
                        EditQuality -= 5 + remove;
                        Player.Current.PostProductionSkill += remove;
                    }
                }

                quality += EditQuality.Value;
            }

            if (Attributes.Contains(VideoAttributes.Cats))
            {
                quality += VideoAttributes.Cats.QualityIncrease;
            }

            quality = Math.Max(quality, 0);
            quality = Math.Min(quality, 100);

            //Imperfection bias
            if (quality == 100)
            {
                if (!QualityBias.HasValue)
                {
                    QualityBias = RandomHelpers.RandomInt(5);
                }
                quality -= QualityBias.Value;
            }

            Quality = quality;
            return Quality.Value;
        }

        private List<YouTubeSearchId> _videoIds;
        public async void FetchRandomImage(bool useCategory = true)
        {
            if (_videoIds != null)
            {
                DoFetchImage();
                return;
            }

            int randomImageCount = 50;
            var searchString = useCategory ? String.Format("{0},{1}", Category.GetString(), Name) : Name;
            var response = await WebClientHelpers.Download<YouTubeSearchResponse>(YouTubeAPI.GetRandomImages(searchString, randomImageCount)); 
            if (response != null && response.Feed != null && response.Feed.Entries != null && response.Feed.Entries.Count > 0)
            {
                _videoIds = new List<YouTubeSearchId>();
                foreach (var entry in response.Feed.Entries)
                {
                    _videoIds.Add(entry.MediaGroup.SearchId);
                }
                DoFetchImage();
            }
            else
            {
                //Limit to category
                var secondResponse = await WebClientHelpers.Download<YouTubeSearchResponse>(YouTubeAPI.GetRandomImages(Category.GetString(), randomImageCount));
                if (secondResponse != null && secondResponse.Feed != null && secondResponse.Feed.Entries != null && secondResponse.Feed.Entries.Count > 0)
                {
                    _videoIds = new List<YouTubeSearchId>();
                    foreach (var entry in secondResponse.Feed.Entries)
                    {
                        _videoIds.Add(entry.MediaGroup.SearchId);
                    }
                    DoFetchImage();
                }
                else
                {
                    var file = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/InternetDown.jpg"));
                    var buf = await FileIO.ReadBufferAsync(file);
                    var bytes = new byte[buf.Length];
                    var dr = DataReader.FromBuffer(buf);
                    dr.ReadBytes(bytes);
                    ImageBytes = bytes;

                    if (OnImageFetched != null)
                        OnImageFetched();
                }
            }

            if (useCategory)
                _videoIds = null;
        }

        private async void DoFetchImage()
        {
            var randomInt = RandomHelpers.RandomInt(_videoIds.Count);
            YouTubeSearchId youTubeVideoId = _videoIds[randomInt];

            Uri photoUri = YouTubeAPI.GetPhotoUri(youTubeVideoId);

            var stream = await WebClientHelpers.DownloadImage(photoUri);
            if (stream != null)
            {
                YouTubeVideoId = youTubeVideoId.VideoId;
                ImageBytes = StreamHelpers.StreamToBytes(stream);
            }

            if (OnImageFetched != null)
                OnImageFetched();
        }
    }
}