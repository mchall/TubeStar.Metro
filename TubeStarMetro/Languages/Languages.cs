using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace TubeStarMetro
{
    [DataContract(Namespace = "")]
    public class CustomText
    {
        [DataMember]
        public EnglishStrings Id { get; set; }

        [DataMember]
        public string Text { get; set; }
    }

    public enum Language
    {
        English = 1,
        German = 2,
        Polish = 3,
        Russian = 4,
        Swedish = 5,
        French = 6,
    }

    public static class Languages
    {
        private static Dictionary<EnglishStrings, string> _customLanguage;
        private static Dictionary<CommentStrings, string> _comments;

        public static Language CurrentLanguage { get; private set; }

        static Languages()
        {
            CurrentLanguage = Language.English;
            _customLanguage = new Dictionary<EnglishStrings, string>();
            _comments = new Dictionary<CommentStrings, string>();
        }

        public async static Task SetLanguage(Language language)
        {
            CurrentLanguage = language;
            _customLanguage = new Dictionary<EnglishStrings, string>();

            if (language == Language.English)
                return;

            var file = await Package.Current.InstalledLocation.GetFileAsync(String.Format(@"Languages\Translations\{0}.xml", language.ToString()));
            string xml = await Windows.Storage.FileIO.ReadTextAsync(file);

            var bytes = System.Text.Encoding.UTF8.GetBytes(xml);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                var ser = new DataContractSerializer(typeof(CustomText[]));
                var data = (CustomText[])ser.ReadObject(ms);

                foreach (var item in data)
                {
                    _customLanguage[item.Id] = item.Text;
                }
            }
        }

        public static string Translate(this EnglishStrings english)
        {
            if (!_customLanguage.ContainsKey(english))
                _customLanguage[english] = EnumHelpers.GetAttribute<DescriptionAttribute>(english).Description;
            return _customLanguage[english];
        }

        public static string Translate(this CommentStrings comment)
        {
            if (!_comments.ContainsKey(comment))
                _comments[comment] = EnumHelpers.GetAttribute<DescriptionAttribute>(comment).Description;
            return _comments[comment];
        }
    }
}