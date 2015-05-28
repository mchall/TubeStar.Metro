using System;
using System.Runtime.Serialization;
using Windows.UI;

namespace TubeStarMetro
{
    [KnownType(typeof(EditVideo))]
    [KnownType(typeof(Job))]
    [KnownType(typeof(BowToRobotRulers))]
    [KnownType(typeof(ShootVideo))]
    [KnownType(typeof(Study))]
    [KnownType(typeof(StudyAudienceAnalysisI))]
    [KnownType(typeof(StudyAudienceAnalysisII))]
    [KnownType(typeof(StudyPostProductionI))]
    [KnownType(typeof(StudyPostProductionII))]
    [KnownType(typeof(StudyPostProductionIII))]
    [KnownType(typeof(StudyProductionI))]
    [KnownType(typeof(StudyProductionII))]
    [KnownType(typeof(StudyProductionIII))]
    [KnownType(typeof(StudyQualityAnalysis))]
    public abstract class TaskBase
    {
        public abstract string Name { get; }
        public abstract Color Color { get; }
        public abstract int HoursToComplete { get; }
        public abstract TaskType TaskType { get; }

        public int ExtraHours { get; set; }
        public int HoursPutIn { get; set; }

        public bool IsCompleted
        {
            get { return HoursLeft < 1; }
        }

        public int HoursLeft
        {
            get { return HoursToComplete + ExtraHours - HoursPutIn; }
        }
    }
}