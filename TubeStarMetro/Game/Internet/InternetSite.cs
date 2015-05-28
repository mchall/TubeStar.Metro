using System;
using System.Collections.Generic;
using System.Linq;

namespace TubeStarMetro
{
    public class InternetSite : Node
    {
        public int OwnerId { get; set; }
        public VideoCategory Topic { get; set; }

        public InternetSite()
            : base()
        { }

        public InternetSite(int id, InternetUser owner)
            : base(id)
        {
            OwnerId = owner.Id;
            Topic = owner.PrimaryIntrest;
        }
    }
}