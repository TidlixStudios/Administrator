using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrator.Notifiers.Studios
{
    public class StudiosEngine
    {
        private string channelID = "UC9gt5OJbnyqAAPWa3DacTXw";
        private string apiKey = "AIzaSyCBJiEBqZXl1OEdng9YC3u1qOkw76VKJdU";
        private StudiosVideo _video = new StudiosVideo();

        public StudiosVideo GetLatestVideo ()
        {
            string videoId;
            string videoUrl;
            string videoTitle;
            string thumbnail;
            DateTime? videoPublishedAt;

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = "StudiosNotifier"
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

                    return new StudiosVideo()
                    {
                        videoId = videoId,
                        videoTitle = videoTitle,
                        videoUrl = videoUrl,
                        thumbnail = thumbnail,
                        publishedAt = videoPublishedAt,
                    };
                }
                return null;
            }
            return null;
        }
    }
}
