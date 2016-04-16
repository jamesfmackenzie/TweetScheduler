using System;
using System.Collections.Generic;
using System.Linq;

namespace TweetScheduler.Model
{
    internal class TweetDatabase
    {
        private List<Tweet> _tweets;

        public List<Tweet> ScheduledTweets
        {
            get { return Tweets.Where(t => !t.Posted && t.ScheduledDateTime > DateTime.Now).ToList(); }
        }

        public List<Tweet> UnscheduledTweets
        {
            get
            {
                return
                    Tweets.Where(t => !t.Posted && (t.ScheduledDateTime == null || t.ScheduledDateTime < DateTime.Now))
                        .ToList();
            }
        }

        public List<Tweet> PostedTweets
        {
            get { return Tweets.Where(t => t.Posted).ToList(); }
        }

        private List<Tweet> Tweets
        {
            get
            {
                if (_tweets == null)
                {
                    _tweets = new List<Tweet>();
                }

                return _tweets;
            }
        }

        public void AddTweet(Tweet t)
        {
            _tweets.Add(t);
        }
    }
}