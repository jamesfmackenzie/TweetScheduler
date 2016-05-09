using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using TweetScheduler.Model;
using TweetScheduler.Repository;

namespace TweetScheduler.ViewModel
{
    class ScheduledTweetsViewModel : ObservableObject
    {
        private List<Tweet> _scheduledTweets;
        private TweetRepository _scheduledTweetRepository = new TweetRepository();

        public ScheduledTweetsViewModel()
        {
            _scheduledTweetRepository.SaveOrUpdate(new Tweet() { Status = "fooooooo" });
            ScheduledTweets = _scheduledTweetRepository.GetAll().ToList();
        }

        public List<Tweet> ScheduledTweets
        {
            get { return _scheduledTweets; }
            set {
                Set(() => ScheduledTweets, ref _scheduledTweets, value);
            }
        }
    }
}
