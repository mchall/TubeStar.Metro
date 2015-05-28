using System;
using System.Collections.Generic;
using System.Linq;

namespace TubeStarMetro
{
    public class InternetUser : Node
    {
        public VideoCategory PrimaryIntrest { get; set; }
        public VideoCategory SecondaryIntrest { get; set; }

        public List<Guid> FanBoyChannels { get; set; }

        public InternetUser()
        { }

        public InternetUser(int id)
            : base(id)
        {
            FanBoyChannels = new List<Guid>();

            PrimaryIntrest = RandomHelpers.RandomVideoCategory;
            SecondaryIntrest = RandomHelpers.RandomVideoCategory;
        }
    }
}