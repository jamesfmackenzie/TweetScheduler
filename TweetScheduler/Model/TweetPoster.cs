using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Threading;
using TweetScheduler.ViewModel;

namespace TweetScheduler.Model
{
    // TODO: move to Service layer?
    // TODO: logging
    // TODO: long running background thread = bad
    internal class TweetPoster
    {
        private readonly TweetService _tweetService = new TweetService();
        private readonly MainWindowViewModel _vm;

        public TweetPoster(List<Tweet> tweets, MainWindowViewModel vm)
        {
            Tweets = tweets;
            _vm = vm;

            Task.Factory.StartNew(BackGroundTask);
        }

        public List<Tweet> Tweets { get; set; }

        private void BackGroundTask()
        {
            while (true)
            {
                if (Tweets != null)
                {
                    foreach (var tweet in Tweets)
                    {
                        if (DateTime.Now > tweet.ScheduledDateTime)
                        {
                            _tweetService.PostTweet(tweet.Status, tweet.MediaUrls);

                            DispatcherHelper.CheckBeginInvokeOnUI(
                                () =>
                                {
                                    // Dispatch back to the main thread
                                    tweet.Posted = true;

                                    // TODO: remove hack
                                    _vm.on_tweetCollectionChanged(null, null);
                                });
                        }
                    }
                }
                Thread.Sleep(30000);
            }
        }
    }
}