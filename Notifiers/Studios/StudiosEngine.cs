using Administrator.Config;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
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
        private StudiosVideo _video = new StudiosVideo();

        private DateTime RateLimitReset;

        public StudiosVideo GetLatestVideo ()
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
                ApplicationName = "StudiosNotifier"
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.ChannelId = channelID;
            searchListRequest.MaxResults = 1;
            searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;

            var searchListResponse = new SearchListResponse();
            try
            {
                searchListResponse = searchListRequest.Execute();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Studio Notifier] Error: {e.Message}");

                if (e.Message == "The service youtube has thrown an exception. HttpStatusCode is Forbidden. The request cannot be completed because you have exceeded your <a href=\"/youtube/v3/getting-started#quota\">quota</a>.")
                {
                    var DT = DateTime.UtcNow.AddDays(1);
                    RateLimitReset = new DateTime(DT.Year, DT.Month, DT.Day, 0, 0, 0);
                    Console.WriteLine($"[Studio Notifier] Youtube Rate Limit hitted. Waiting until {RateLimitReset} UTC before doing next request!");
                }
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }

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
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
