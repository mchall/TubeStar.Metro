using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Windows.UI;

namespace TubeStarMetro
{
    public enum SkillModifierType
    {
        Shooting,
        PostProduction,
        VideoAttribute,
        ViewQuality,
        None,
    }

    [KnownType(typeof(StudyAudienceAnalysisI))]
    [KnownType(typeof(StudyAudienceAnalysisII))]
    [KnownType(typeof(StudyPostProductionI))]
    [KnownType(typeof(StudyPostProductionII))]
    [KnownType(typeof(StudyPostProductionIII))]
    [KnownType(typeof(StudyProductionI))]
    [KnownType(typeof(StudyProductionII))]
    [KnownType(typeof(StudyProductionIII))]
    [KnownType(typeof(StudyQualityAnalysis))]
    public abstract class Study : TaskBase
    {
        public abstract Study Prerequisite { get; }
        public abstract int Cost { get; }

        public abstract int SkillModifier { get; }
        public abstract SkillModifierType SkillModifierType { get; }

        public override TaskType TaskType
        {
            get { return TaskType.Study; }
        }

        public override Color Color
        {
            get { return Colors.DodgerBlue; }
        }
    }
}