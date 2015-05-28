using System;

namespace TubeStarMetro
{
    public class LearnFromMistakes : VideoAttribute
    {
        public override string Name
        {
            get { return EnglishStrings.LearnFromMistakesAttribute.Translate(); }
        }

        public override string Description
        {
            get { return EnglishStrings.LearnFromMistakesAttributeDescription.Translate(); }
        }

        public override int Cost
        {
            get { return 1; }
        }
    }
}