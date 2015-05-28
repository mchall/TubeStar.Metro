using System;
using System.Collections.Generic;
using Windows.Storage;

namespace TubeStarMetro
{
    public static class Settings
    {
        public static VideoCategory? LastCategory { get; set; }
        public static Channel LastChannel { get; set; }
    }
}