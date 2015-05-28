using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TubeStarMetro
{
    public class YouTubeAPI
    {
        private const string ApiKey = "AI39si7sa95VulEV8_pn17ka6EHFQv-1vdtkl8Gol73dU6dF5oYCT0SvolVHeTR6M3URdQEJ2P3QOPk5eGYRtU2b1lOFYGAb8Q";

        public static Uri GetRandomImages(string search, int max)
        {
            search = search.Replace(' ', ',');
            var uriString = String.Format("http://gdata.youtube.com/feeds/api/videos?q={0}&max-results={1}&v=2&fields=entry(media:group(yt:videoid))&alt=json&key={2}",
                HttpHelpers.UrlEncoding(search), max, ApiKey);
            return new Uri(uriString);
        }

        public static Uri GetRandomComments(string videoId, int max)
        {
            var uriString = String.Format("http://gdata.youtube.com/feeds/api/videos/{0}/comments?alt=json&max-results={1}&fields=entry(content)&key={2}",
                videoId, max, ApiKey);
            return new Uri(uriString);
        }

        public static Uri GetPhotoUri(YouTubeSearchId id)
        {
            return new Uri(String.Format("http://i.ytimg.com/vi/{0}/hqdefault.jpg", id.VideoId));
        }
    }

    [JsonObject()]
    public class YouTubeCommentResponse
    {
        [JsonProperty("feed")]
        public YouTubeCommentFeed Feed { get; set; }
    }

    [JsonObject()]
    public class YouTubeCommentFeed
    {
        [JsonProperty("entry")]
        public List<YouTubeCommentEntry> Entries { get; set; }
    }

    [JsonObject()]
    public class YouTubeCommentEntry
    {
        [JsonProperty("content")]
        public YouTubeCommentGroup Content { get; set; }
    }

    [JsonObject()]
    public class YouTubeCommentGroup
    {
        [JsonProperty("$t")]
        public string Comment { get; set; }
    }

    [JsonObject()]
    public class YouTubeSearchResponse
    {
        [JsonProperty("feed")]
        public YouTubeSearchFeed Feed { get; set; }
    }

    [JsonObject()]
    public class YouTubeSearchFeed
    {
        [JsonProperty("entry")]
        public List<YouTubeSearchEntry> Entries { get; set; }
    }

    [JsonObject()]
    public class YouTubeSearchEntry
    {
        [JsonProperty("media$group")]
        public YouTubeSearchMediaGroup MediaGroup { get; set; }
    }

    [JsonObject()]
    public class YouTubeSearchMediaGroup
    {
        [JsonProperty("yt$videoid")]
        public YouTubeSearchId SearchId { get; set; }
    }

    [JsonObject()]
    public class YouTubeSearchId
    {
        [JsonProperty("$t")]
        public string VideoId { get; set; }
    }
}