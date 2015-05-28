using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace TubeStarMetro
{
    [DataContract]
    public enum VideoCategory
    {
        [EnumMember]
        UkuleleCover = 1,

        [EnumMember]
        Technology = 2,

        [EnumMember]
        MusicVideo = 3,

        [EnumMember]
        TheWeirdSide = 4,

        [EnumMember]
        Sports = 5,

        [EnumMember]
        Comedy = 6,

        [EnumMember]
        Gaming = 7,

        [EnumMember]
        HowTo = 8,

        [EnumMember]
        Hauls = 9,

        [EnumMember]
        Cats = 10,

        [EnumMember]
        ConspiraryTheories = 11,

        [EnumMember]
        Vlog = 12,

        [EnumMember]
        Accidents = 13,

        [EnumMember]
        NonProfit = 14,

        [EnumMember]
        Anime = 15,

        [EnumMember]
        Movies = 16,

        [EnumMember]
        Creepypasta = 17,
    }

    public static class VideoCategoryHelpers
    {
        public static string GetString(this VideoCategory category)
        {
            switch (category)
            {
                case VideoCategory.Accidents: return EnglishStrings.Fails.Translate();
                case VideoCategory.Anime: return EnglishStrings.Anime.Translate();
                case VideoCategory.Cats: return EnglishStrings.Cats.Translate();
                case VideoCategory.Comedy: return EnglishStrings.Comedy.Translate();
                case VideoCategory.ConspiraryTheories: return EnglishStrings.ConspiraryTheory.Translate();
                case VideoCategory.Creepypasta: return EnglishStrings.CreepyPasta.Translate();
                case VideoCategory.Gaming: return EnglishStrings.Gaming.Translate();
                case VideoCategory.Hauls: return EnglishStrings.ShoppingHaul.Translate();
                case VideoCategory.HowTo: return EnglishStrings.HowTo.Translate();
                case VideoCategory.Movies: return EnglishStrings.Movies.Translate();
                case VideoCategory.MusicVideo: return EnglishStrings.MusicVideo.Translate();
                case VideoCategory.NonProfit: return EnglishStrings.NonProfit.Translate();
                case VideoCategory.Sports: return EnglishStrings.Sports.Translate();
                case VideoCategory.TheWeirdSide: return EnglishStrings.TheWeirdSide.Translate();
                case VideoCategory.Technology: return EnglishStrings.Technology.Translate();
                case VideoCategory.UkuleleCover: return EnglishStrings.UkuleleCover.Translate();
                case VideoCategory.Vlog: return EnglishStrings.Vlog.Translate();
            }
            return String.Empty;
        }
    }
}