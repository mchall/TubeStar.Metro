using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TubeStarMetro
{
    [KnownType(typeof(VideoCameraI))]
    [KnownType(typeof(VideoCameraII))]
    [KnownType(typeof(EditingSoftwareI))]
    [KnownType(typeof(EditingSoftwareII))]
    [KnownType(typeof(Lawyer))]
    [KnownType(typeof(Consultant))]
    [KnownType(typeof(Loan))]
    public abstract class StoreItem
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract int Cost { get; }
        public abstract string ImageName { get; }

        public abstract int SkillModifier { get; }
        public abstract SkillModifierType SkillModifierType { get; }

        public bool Purchased { get; set; }
    }
}