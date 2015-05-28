using System;
using Windows.UI;

namespace TubeStarMetro
{
    public class Job : TaskBase
    {
        public override TaskType TaskType
        {
            get { return TaskType.Job; }
        }

        public override string Name
        {
            get { return EnglishStrings.Job.Translate(); }
        }

        public override Color Color
        {
            get { return Colors.Crimson; }
        }

        public override int HoursToComplete
        {
            get { return 8; }
        }
    }
}