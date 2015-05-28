using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace TubeStarMetro
{
    [DataContract]
    public enum EnglishStrings
    {
        [EnumMember, Description("Fails")]
        Fails,

        [EnumMember, Description("Anime")]
        Anime,

        [EnumMember, Description("Cats")]
        Cats,

        [EnumMember, Description("Comedy")]
        Comedy,

        [EnumMember, Description("Conspiracy theory")]
        ConspiraryTheory,

        [EnumMember, Description("Creepypasta")]
        CreepyPasta,

        [EnumMember, Description("Gaming")]
        Gaming,

        [EnumMember, Description("Shopping haul")]
        ShoppingHaul,

        [EnumMember, Description("How to ...")]
        HowTo,

        [EnumMember, Description("Movies")]
        Movies,

        [EnumMember, Description("Music video")]
        MusicVideo,

        [EnumMember, Description("Non profit")]
        NonProfit,

        [EnumMember, Description("Sports")]
        Sports,

        [EnumMember, Description("The weird side")]
        TheWeirdSide,

        [EnumMember, Description("Technology")]
        Technology,

        [EnumMember, Description("Ukulele cover")]
        UkuleleCover,

        [EnumMember, Description("Vlog")]
        Vlog,

        [EnumMember, Description("OK")]
        Ok,

        [EnumMember, Description("Cancel")]
        Cancel,

        [EnumMember, Description("Something went horribly wrong!\nOr maybe only slightly wrong...\nAnyway, sorry! I couldn't save the game!")]
        SaveFail,

        [EnumMember, Description("Something went horribly wrong!\nOr maybe only slightly wrong...\nAnyway, sorry! I couldn't load the game!")]
        LoadFail,

        [EnumMember, Description("Channel '{0}' has been suspended due to a suspected violation of the terms and conditions!")]
        ChannelSuspended,

        [EnumMember, Description("Your computer has crashed!\n\nAll unreleased videos have been lost!")]
        ComputerCrash,

        [EnumMember, Description("The economy is failing!\n\nYour cost of living has increased!")]
        CostOfLivingIncrease,

        [EnumMember, Description("Your work has been outsourced!\n\nYou're fired!")]
        Fired,

        [EnumMember, Description("Some embarrassing information about you has surfaced!\n\nYou've lost {0} subscribers!")]
        LoseSubscribers,

        [EnumMember, Description("Mmm, yeah, I'm going to have to go ahead and ask you to work overtime.\n\nSo if you could go ahead and do that, that'd be great!")]
        WorkOvertime,

        [EnumMember, Description("Video '{0}' has been suspended due to suspected copyright infringement!")]
        VideoSuspended,

        [EnumMember, Description("You searched through your sofa and found {0} change")]
        SpareChange,

        [EnumMember, Description("You've been promoted at work!\n\nYou earn more money while working longer hours!")]
        Promoted,

        [EnumMember, Description("The robots have risen and taken over the earth!\n\nBow down to your new robot rulers.")]
        RobotRising,

        [EnumMember, Description("You've won ${0} in a beauty contest!")]
        BeautyContest,

        [EnumMember, Description("Channel '{0}' has had it's suspension lifted!")]
        ChannelSuspensionLifted,

        [EnumMember, Description("The economy is recovering!\n\nYour cost of living has decreased!")]
        CostOfLivingDecreased,

        [EnumMember, Description("You've been given a scholarship to {0}!")]
        FreeStudy,

        [EnumMember, Description("You've been featured on a website!\n\nYou've gained {0} subscribers!")]
        GainSubscribers,

        [EnumMember, Description("A video '{0}' has had it's suspension lifted!")]
        VideoSuspensionLifted,

        [EnumMember, Description("Random event")]
        RandomEvent,

        [EnumMember, Description("Study 'Video Attribution Technology I'")]
        StudyAudienceAnalysis1,

        [EnumMember, Description("Study 'Video Attribution Technology II'")]
        StudyAudienceAnalysis2,

        [EnumMember, Description("Study 'Post Production I'")]
        StudyPostProduction1,

        [EnumMember, Description("Study 'Post Production II'")]
        StudyPostProduction2,

        [EnumMember, Description("Study 'Post Production III'")]
        StudyPostProduction3,

        [EnumMember, Description("Study 'Production I'")]
        StudyProduction1,

        [EnumMember, Description("Study 'Production II'")]
        StudyProduction2,

        [EnumMember, Description("Study 'Production III'")]
        StudyProduction3,

        [EnumMember, Description("Study 'Quality Analysis'")]
        StudyQualityAnalysis,

        [EnumMember, Description("Study 'Statistical Analysis'")]
        StudyStatisticalAnalysis,

        [EnumMember, Description("Mandatory Bowing to Robot Rulers")]
        MandatoryBowToRobotRulers,

        [EnumMember, Description("'{0}' post production")]
        EditVideoTask,

        [EnumMember, Description("Job")]
        Job,

        [EnumMember, Description("'{0}' video shoot")]
        ShootVideoTask,

        [EnumMember, Description("May contain cats")]
        CatsAttribute,

        [EnumMember, Description("Video quality increased slightly")]
        CatsAttributeDescription,

        [EnumMember, Description("Copycat")]
        CopycatAttribute,

        [EnumMember, Description("{0} free initial views")]
        CopycatAttributeDescription,

        [EnumMember, Description("Fanboy bait")]
        FanboyBaitAttribute,

        [EnumMember, Description("If the video quality is over 75%, no current subscribers will unsubscribe")]
        FanboyBaitAttributeDescription,

        [EnumMember, Description("Genre buster")]
        GenreBusterAttribute,

        [EnumMember, Description("Video category is less of a factor in likeability")]
        GenreBusterAttributeDescription,

        [EnumMember, Description("Hypnotic")]
        HypnoticAttribute,

        [EnumMember, Description("Higher chance of subscriptions")]
        HypnoticAttributeDescription,

        [EnumMember, Description("Learning from mistakes")]
        LearnFromMistakesAttribute,

        [EnumMember, Description("Player stats slightly increased at cost of video quality")]
        LearnFromMistakesAttributeDescription,

        [EnumMember, Description("Memetic")]
        MemeticAttribute,

        [EnumMember, Description("Higher chance of sharing")]
        MemeticAttributeDescription,

        [EnumMember, Description("Product Placement")]
        ProductPlacementAttribute,

        [EnumMember, Description("Earn more from advertising per view")]
        ProductPlacementAttributeDescription,

        [EnumMember, Description("Better second time")]
        SecondTimeAttribute,

        [EnumMember, Description("The video can be viewed multiple times")]
        SecondTimeAttributeDescription,

        [EnumMember, Description("So bad it's good")]
        SoBadAttribute,

        [EnumMember, Description("Chance of video being shared if the video is disliked")]
        SoBadAttributeDescription,

        [EnumMember, Description("Crowdfunding")]
        CrowdFundingAttribute,

        [EnumMember, Description("Instead of a subscription you get {0}")]
        CrowdFundingAttributeDescription,

        [EnumMember, Description("Above board")]
        AboveBoardAttribute,

        [EnumMember, Description("The video cannot be suspended")]
        AboveBoardAttributeDescription,

        [EnumMember, Description("Unreleased Videos")]
        UnreleasedVideos,

        [EnumMember, Description("Continue game")]
        ContinueGame,

        [EnumMember, Description("New game")]
        NewGame,

        [EnumMember, Description("Tutorial")]
        Tutorial,

        [EnumMember, Description("Credits")]
        Credits,

        [EnumMember, Description("Click a video thumbnail to watch the YouTube video")]
        TitlePageTip1,

        [EnumMember, Description("Hover the mouse over video attributes to see what they do")]
        TitlePageTip2,

        [EnumMember, Description("If you study enough, you can add multiple attributes to a video")]
        TitlePageTip3,

        [EnumMember, Description("'Challenge' mode has random events")]
        TitlePageTip4,

        [EnumMember, Description("Buy views early to get search engine hits later and gain subscribers")]
        TitlePageTip5,

        [EnumMember, Description("A larger subscriber base means more pay per view")]
        TitlePageTip6,

        [EnumMember, Description("The more advertising a channel has, the lower the subscription rate")]
        TitlePageTip7,

        [EnumMember, Description("Quit your job by clicking on it")]
        TitlePageTip8,

        [EnumMember, Description("TIP")]
        Tip,

        [EnumMember, Description("What a loser")]
        WhatALoser,

        [EnumMember, Description("You ran out of money!")]
        OutOfMoney,

        [EnumMember, Description("Death")]
        Death,

        [EnumMember, Description("You died fighting the robot menace.\nYour official cause of death is noted as:\n\n'Exhaustion from overeagerness to bow to robot masters.'")]
        RobotDeath,

        [EnumMember, Description("Confirm")]
        Confirm,

        [EnumMember, Description("Leave the current game?")]
        LeaveGame,

        [EnumMember, Description("Overwrite save")]
        OverwriteSave,

        [EnumMember, Description("A save file already exists!\nOverwrite?")]
        SaveExists,

        [EnumMember, Description("Start day")]
        StartDay,

        [EnumMember, Description("Help")]
        Help,

        [EnumMember, Description("Exit")]
        Exit,

        [EnumMember, Description("Save Game")]
        SaveGame,

        [EnumMember, Description("The game was successfully saved!")]
        SaveGameSuccess,

        [EnumMember, Description("Aren't you missing something?")]
        MissingValueHeader,

        [EnumMember, Description("Enter a name")]
        MissingName,

        [EnumMember, Description("Select a category")]
        MissingCategory,

        [EnumMember, Description("Select a channel")]
        MissingChannel,

        [EnumMember, Description("Select a strategy")]
        MissingStrategy,

        [EnumMember, Description("Select a video")]
        MissingVideo,

        [EnumMember, Description("Enter the username")]
        MissingUserName,

        [EnumMember, Description("Enter the token")]
        MissingToken,

        [EnumMember, Description("per view")]
        PerView,

        [EnumMember, Description("Add channel")]
        AddChannel,

        [EnumMember, Description("Edit channel")]
        EditChannel,

        [EnumMember, Description("Advertising strategy")]
        AdvertisingStrategy,

        [EnumMember, Description("Name")]
        Name,

        [EnumMember, Description("Low")]
        Low,

        [EnumMember, Description("Normal")]
        Normal,

        [EnumMember, Description("High")]
        High,

        [EnumMember, Description("Aggressive")]
        Aggressive,

        [EnumMember, Description("Shoot video")]
        ShootVideo,

        [EnumMember, Description("Edit video")]
        EditVideo,

        [EnumMember, Description("Study")]
        Study,

        [EnumMember, Description("Jumped the gun")]
        TooSoon,

        [EnumMember, Description("There are no videos to edit!")]
        NoVideosToEdit,

        [EnumMember, Description("points left")]
        PointsLeft,

        [EnumMember, Description("How you gonna pay for that?")]
        LowCashHeader,

        [EnumMember, Description("Not enough cash!")]
        LowCashMessage,

        [EnumMember, Description("Hours")]
        Hours,

        [EnumMember, Description("Hours left")]
        HoursLeft,

        [EnumMember, Description("Add video")]
        AddVideo,

        [EnumMember, Description("Attributes")]
        Attributes,

        [EnumMember, Description("Cost")]
        Cost,

        [EnumMember, Description("Category")]
        Category,

        [EnumMember, Description("Suspended")]
        Suspended,

        [EnumMember, Description("Video")]
        Video,

        [EnumMember, Description("Videos")]
        Videos,

        [EnumMember, Description("Add Task")]
        AddTask,

        [EnumMember, Description("Quit your job")]
        QuitJobHeader,

        [EnumMember, Description("Taking this action will quit your job. \nAre you sure?")]
        QuitJobText,

        [EnumMember, Description("Rise up!")]
        RiseUp,

        [EnumMember, Description("You need at least {0} to start a rebellion against our robot masters!")]
        RebellionCashRequired,

        [EnumMember, Description("Taking this action will start an uprising against our robot masters!\nYou have a {0}% chance of success and failure will result in DEATH! \n\nAre you sure?")]
        RebellionStart,

        [EnumMember, Description("Freedom!")]
        Freedom,

        [EnumMember, Description("You find the master switch and switch the master robot race off!\nThat was easy!")]
        RebellionSuccess,

        [EnumMember, Description("Remove task")]
        RemoveTask,

        [EnumMember, Description("Are you sure?")]
        AreYouSure,

        [EnumMember, Description("No refunds!")]
        NoRefunds,

        [EnumMember, Description("Fetching scores...")]
        FetchingScores,

        [EnumMember, Description("Casual")]
        Casual,

        [EnumMember, Description("Challenge")]
        Challenge,

        [EnumMember, Description("Failure")]
        Failure,

        [EnumMember, Description("Hours to complete")]
        HoursToComplete,

        [EnumMember, Description("Prerequisites not met!")]
        PrerequisitesNotMet,

        [EnumMember, Description("Already completed")]
        AlreadyCompleted,

        [EnumMember, Description("Already enrolled")]
        AlreadyEnrolled,

        [EnumMember, Description("Study selection")]
        StudySelection,

        [EnumMember, Description("Upload video")]
        UploadVideo,

        [EnumMember, Description("Fetch new image")]
        FetchNewImage,

        [EnumMember, Description("Image")]
        Image,

        [EnumMember, Description("Buy views")]
        BuyViews,

        [EnumMember, Description("Channel")]
        Channel,

        [EnumMember, Description("Views")]
        Views,

        [EnumMember, Description("None")]
        None,

        [EnumMember, Description("Comments")]
        Comments,

        [EnumMember, Description("Quality")]
        Quality,

        [EnumMember, Description("N/A")]
        NotApplicable,

        [EnumMember, Description("Likes")]
        Likes,

        [EnumMember, Description("Dislikes")]
        Dislikes,

        [EnumMember, Description("Subscribers")]
        Subscribers,

        [EnumMember, Description("Channel Income")]
        ChannelIncome,

        [EnumMember, Description("Delete")]
        Delete,

        [EnumMember, Description("Are you sure you want to delete this video?")]
        DeleteVideo,

        [EnumMember, Description("Where to?")]
        WhereTo,

        [EnumMember, Description("Create a channel to upload to!")]
        ChannelNeeded,

        [EnumMember, Description("Video comments")]
        VideoComments,

        [EnumMember, Description("Top comments")]
        TopComments,

        [EnumMember, Description("Video Manager")]
        VideoManager,

        [EnumMember, Description("Daily Planner")]
        DailyPlanner,

        [EnumMember, Description("Stats")]
        Stats,

        [EnumMember, Description("Subscriber History")]
        SubscriberHistory,

        [EnumMember, Description("Daily Income and Expenses")]
        DailyIncome,

        [EnumMember, Description("Generic Camcorder XYZ-85")]
        VideoCameraIStoreItem,

        [EnumMember, Description("It's better than a webcam...")]
        VideoCameraIStoreItemDescription,

        [EnumMember, Description("Super Camcorder Pro Deluxe HD")]
        VideoCameraIIStoreItem,

        [EnumMember, Description("Best video camera on the market! Used by all the pros!")]
        VideoCameraIIStoreItemDescription,

        [EnumMember, Description("Edit-Me Video Director 4")]
        EditingSoftwareIStoreItem,

        [EnumMember, Description("Better than the one that comes free with my OS...")]
        EditingSoftwareIStoreItemDescription,

        [EnumMember, Description("Mega-Director Ultra Deluxe Edition 12")]
        EditingSoftwareIIStoreItem,

        [EnumMember, Description("The ultimate in video editing software! 20 different star swipes to choose from!")]
        EditingSoftwareIIStoreItemDescription,

        [EnumMember, Description("Wright & Wrong Attorneys")]
        LawyerStoreItem,

        [EnumMember, Description("For all your legal needs! We'll unblock any suspended video or channel for a small fee of {0}!")]
        LawyerStoreItemDescription,

        [EnumMember, Description("Bob Boskins Consulting")]
        ConsultantStoreItem,

        [EnumMember, Description("With my expert guidance, I'll make you a TubeStar! I also charge an additional {0} per video!")]
        ConsultantStoreItemDescription,

        [EnumMember, Description("Buy")]
        Buy,

        [EnumMember, Description("Online Store")]
        OnlineStore,

        [EnumMember, Description("(Click to Hire Lawyer)")]
        SuspendedHireLawyer,

        [EnumMember, Description("Do you want to pay our fee of {0} to lift this video's suspension?")]
        RemoveVideoSuspension,

        [EnumMember, Description("Do you want to pay our fee of {0} to lift this channel's suspension?")]
        RemoveChannelSuspension,

        [EnumMember, Description("Shylock's Loans")]
        LoanStoreItem,

        [EnumMember, Description("Immediate payout of {0} guaranteed!! Call today! \nPay back {1} per day. {2}% interest.")]
        LoanStoreItemDescription,

        [EnumMember, Description("Edit")]
        Edit,

        [EnumMember, Description("Save")]
        Save,

        [EnumMember, Description("Born to Rule")]
        BornToRule,

        [EnumMember, Description("Buy for {0}?")]
        BuyItem,

        [EnumMember, Description("Next")]
        Next,

        [EnumMember, Description("Community")]
        Community,
    }
}