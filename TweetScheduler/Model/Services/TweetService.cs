using System.Collections.Generic;
using System.Linq;
using System.Net;
using Tweetinvi;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Parameters;

namespace TweetScheduler.Model.Services
{
    internal class TweetService
    {
        // TODO: move to separate Service folder/layer?
        private const string KEY = "INSERT KEY";
        private const string SECRET = "INSERT SECRET";
        private const string TOKEN = "INSERT TOKEN";
        private const string TOKENSECRET = "INSERT TOKEN SECRET";

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