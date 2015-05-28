using System;

namespace TubeStarMetro
{
    public static class CategoryHelpers
    {
        public static bool CheckInterest(InternetUser user, VideoCategory category)
        {
            if (user.PrimaryIntrest == category)
            {
                return true;
            }
            else if (user.SecondaryIntrest == category)
            {
                return RandomHelpers.RandomBool(); //50% chance
            }
            else return RandomHelpers.Chance(5); //5% chance
        }

        public static bool CheckInterest(InternetUser user, Video video)
        {
            var result = CheckInterest(user, video.Category);
            if (!result && video.Attributes.Contains(VideoAttributes.GenreBuster))
            {
                return RandomHelpers.Chance(35); //extra 35% chance
            }
            return result;
        }
    }
}