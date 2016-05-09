using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TweetScheduler.DataAccess;
using TweetScheduler.Model;
using TweetScheduler.Repository;

namespace TweetScheduler.Service
{
    // TODO: logging
    internal class TweetPoster
    {
        private readonly TweetRepository _tweetRepository;
        private readonly TweetService _tweetService;

        public TweetPoster(TweetRepository tweetRepository)
        {
            _tweetService = new TweetService();
            _tweetRepository = tweetRepository;

            Task.Factory.StartNew(PostTweetsInBackGround);
        }

        private void PostTweetsInBackGround()
        {
            //TODO: long running background tasks are bad. Should use some kind of timer for this instead.

            while (true)
            {
                try
                {
                    var tweets = _tweetRepository.GetAll().ToList();
                    var tweetsToUpdate = new List<Tweet>();

                    foreach (var tweet in tweets)
                    {
                        if (!tweet.Posted && tweet.ScheduledDateTime != null && DateTime.Now > tweet.ScheduledDateTime)
                        {
                            _tweetService.PostTweet(tweet.Status, tweet.MediaUrls);
                            tweet.Posted = true;
                            tweetsToUpdate.Add(tweet);
                        }
                    }

                    tweetsToUpdate.ForEach(t => _tweetRepository.SaveOrUpdate(t));
                }
                catch (Exception e)
                {
                    // TODO: should really marshall this to the foreground thread
                    MessageBox.Show(e.ToString());
                }

                Thread.Sleep(30000);
            }
        }
    }
}