using System;
using System.Collections.Generic;
using System.Linq;

namespace TubeStarMetro
{
    public static class RandomHelpers
    {
        private const int _videoCategoryLength = 18;
        private static Random _random;

        static RandomHelpers()
        {
            _random = new Random();
        }

        public static VideoCategory RandomVideoCategory
        {
            get { return (VideoCategory)_random.Next(1, _videoCategoryLength + 1); }
        }

        public static int RandomInt(int max)
        {
            return _random.Next(max);
        }

        public static bool Chance(int percentage = 100)
        {
            percentage = Math.Min(percentage, 100);
            percentage = Math.Max(percentage, 0);

            return _random.Next(100) <= percentage;
        }

        public static bool RandomBool()
        {
            return _random.NextDouble() > 0.5;
        }
    }
}