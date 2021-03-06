﻿using System;
using Windows.UI;

namespace TubeStarMetro
{
    public class ShootVideo : TaskBase
    {
        public const int MinimumShootTime = 2;

        public Video Video { get; set; }

        public ShootVideo()
        { }

        public ShootVideo(Video video)
        {
            Video = video;
        }

        public override TaskType TaskType
        {
            get { return TaskType.ShootVideo; }
        }

        public override string Name
        {
            get { return String.Format(EnglishStrings.ShootVideoTask.Translate(), Video.Name); }
        }

        public override Color Color
        {
            get { return Colors.Green; }
        }

        public override int HoursToComplete
        {
            get { return MinimumShootTime; }
        }
    }
}