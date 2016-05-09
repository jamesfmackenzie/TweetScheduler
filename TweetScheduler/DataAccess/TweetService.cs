using System.Collections.Generic;
using System.Linq;
using System.Net;
using Tweetinvi;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Parameters;

namespace TweetScheduler.DataAccess
{
    //TODO: sort out class visibility
    // TODO: tests!!!
    internal class TweetService
    {
        private const string KEY = "INSERT CONSUMER KEY HERE";
        private const string SECRET = "INSERT CONSUMER SECRET HERE";
        private const string TOKEN = "INSERT TOKEN HERE";
        private const string TOKENSECRET = "INSERT TOKEN SECRET HERE";

        public void PostTweet(string status, List<string> imageUrls)
        {
            var webClient = new WebClient();

            var credentials = new TwitterCredentials(KEY, SECRET, TOKEN, TOKENSECRET);

            var tweet = Auth.ExecuteOperationWithCredentials(credentials, () =>
            {
                var images = imageUrls.Select(u => Upload.UploadImage(webClient.DownloadData(u))).ToList();

                return Tweetinvi.Tweet.PublishTweet(status, new PublishTweetOptionalParameters
                {
                    Medias = images
                });
            });
        }
    }
}