using System;
using System.Collections.Generic;
using TweetScheduler.DataAccess;
using TweetScheduler.Model;

namespace TweetScheduler.Repository
{
    public class TweetRepository : IRepository<Tweet>
    {
        private readonly TweetDatabase _tweetDatabase;

        public TweetRepository()
        {
            _tweetDatabase = new TweetDatabase();
        }

        public void Delete(Tweet tweet)
        {
            _tweetDatabase.DeleteById(tweet.Id);
            OnTweetAddedUpdatedOrDeleted();
        }

        public IEnumerable<Tweet> GetAll()
        {
            return _tweetDatabase.RetrieveAll();
        }

        public Tweet SaveOrUpdate(Tweet tweet)
        {
            var existingTweet = _tweetDatabase.RetrieveById(tweet.Id);

            if (existingTweet != null)
            {
                _tweetDatabase.DeleteById(existingTweet.Id);
            }

            _tweetDatabase.Create(tweet);

            OnTweetAddedUpdatedOrDeleted();

            return tweet;
        }

        public event EventHandler TweetAddedUpdatedOrDeleted;

        private void OnTweetAddedUpdatedOrDeleted()
        {
            TweetAddedUpdatedOrDeleted?.Invoke(this, EventArgs.Empty);
        }
    }
}