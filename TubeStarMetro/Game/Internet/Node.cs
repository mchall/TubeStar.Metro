using System;
using System.Collections.Generic;
using System.Linq;

namespace TubeStarMetro
{
    public abstract class Node
    {
        public int Id { get; set; }

        public Node()
        { }

        public Node(int id)
        {
            Id = id;
        }

        #region Equals, GetHashCode overrides

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(this, obj)) return true;

            var other = obj as Node;
            if (other != null && other.Id == this.Id) return true;

            return base.Equals(obj);
        }

        #endregion Equals, GetHashCode overrides
    }
}