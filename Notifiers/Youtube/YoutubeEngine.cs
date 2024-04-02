using Administrator.Config;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrator.Notifiers.Youtube
{
    public class YoutubeEngine
    {
        private string channelID = "UCavdx9Wwwmjmg923LdAuM4w";
        private YoutubeVideo _video = new YoutubeVideo();

        public YoutubeVideo GetLatestVideo ()
        {
            ConfigReader Config = Program.reader;
            string apiKey = Config.googleApiKey;

            string videoId;
            string videoUrl;
            string videoTitle;
            string thumbnail;
            DateTime? videoPublishedAt;

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = "YoutubeNotifier"
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.ChannelId = channelID;
            searchListRequest.MaxResults = 1;
            searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;

            var searchListResponse = searchListRequest.Execute();

            foreach (var searchResult in searchListResponse.Items)
            {
                if (searchResult.Id.Kind == "youtube#video")
                {
                    videoId = searchResult.Id.VideoId;
                    videoUrl = $"https://www.youtube.com/watch?v={videoId}";
                    videoTitle = searchResult.Snippet.Title;
                    thumbnail = searchResult.Snippet.Thumbnails.Default__.Url;
#pragma warning disable CS0618 // Type or member is obsolete
                    videoPublishedAt = searchResult.Snippet.PublishedAt;
#pragma warning restore CS0618 // Type or member is obsolete

                    return new YoutubeVideo()
                    {
                        videoId = videoId,
                        videoTitle = videoTitle,
                        videoUrl = videoUrl,
                        thumbnail = thumbnail,
                        publishedAt = videoPublishedAt
                    };
                }
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }
        
    }
}
