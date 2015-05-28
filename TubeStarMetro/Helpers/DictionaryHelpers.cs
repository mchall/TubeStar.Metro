using System;
using System.Collections.Generic;
using System.Linq;

namespace TubeStarMetro
{
    public static class DictionaryHelpers
    {
        public static int IndexOf<T>(this Dictionary<T, string> data, T value)
        {
            int index = 0;
            foreach (var item in data)
            {
                if (item.Key.Equals(value))
                {
                    return index;
                }
                index++;
            }
            return 0;
        }
    }
}