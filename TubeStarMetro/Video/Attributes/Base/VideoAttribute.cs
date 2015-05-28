using System;
using System.Runtime.Serialization;

namespace TubeStarMetro
{
    [KnownType(typeof(Cats))]
    [KnownType(typeof(Copycat))]
    [KnownType(typeof(FanboyBait))]
    [KnownType(typeof(GenreBuster))]
    [KnownType(typeof(Hypnotic))]
    [KnownType(typeof(LearnFromMistakes))]
    [KnownType(typeof(Memetic))]
    [KnownType(typeof(ProductPlacement))]
    [KnownType(typeof(SecondTime))]
    [KnownType(typeof(SoBad))]
    [KnownType(typeof(Crowdfunding))]
    [KnownType(typeof(AboveBoard))]
    public abstract class VideoAttribute
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract int Cost { get; }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(this, obj)) return true;

            var other = obj as VideoAttribute;
            if (other != null && other.Name == this.Name) return true;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}