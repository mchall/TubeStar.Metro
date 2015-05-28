using System;
using System.Collections.Generic;

namespace TubeStarMetro
{
    public static class ListHelpers
    {
        public static void Add<T>(this List<T> list, T item, int count)
        {
            for (int i = 0; i < count; i++)
            {
                list.Add(item);
            }
        }
    }
}