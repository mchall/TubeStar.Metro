using System;

namespace TubeStarMetro
{
    public class DescriptionAttribute : Attribute
    {
        public string Description { get; private set; }

        public DescriptionAttribute(string description)
            : base()
        {
            Description = description;
        }
    }
}