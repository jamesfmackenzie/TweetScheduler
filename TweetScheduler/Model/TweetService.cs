using System.Collections.Generic;
using System.Linq;
using System.Net;
using Tweetinvi;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Parameters;

namespace TweetScheduler.Model
{
    internal class TweetService
    {
        // TODO: move to separate Service folder/layer?
        public string key = "KEY VALUE";
        public string secret = "SECRET VALUE";
        public string token = "TOKEN VALUE";
        public string tokenSecret = "TOKEN SECRET";

        public string Secret
        {
            get { return secret; }
        }

        public string Token
        {
            get { return token; }
        }

        public string TokenSecret
        {
            get { return tokenSecret; }
        }

        public void PostTweet(string status, List<string> imageUrls)
        {
            var webClient = new WebClient();

            var credentials = new TwitterCredentials(key, secret, token, tokenSecret);

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