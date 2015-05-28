using System;
using System.Linq;
using System.Reflection;

namespace TubeStarMetro
{
    public static class EnumHelpers
    {
        public static T GetAttribute<T>(this Enum enumeration)
            where T : Attribute
        {
            return enumeration.GetType().GetTypeInfo().GetDeclaredField(enumeration.ToString()).GetCustomAttribute<T>();
        }

        public static bool HasAttribute<T>(this Enum enumeration)
            where T : Attribute
        {
            return GetAttribute<T>(enumeration) != null;
        }
    }
}