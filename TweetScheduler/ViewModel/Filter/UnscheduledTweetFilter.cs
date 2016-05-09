using System.Collections.Generic;
using System.Linq;
using TweetScheduler.ViewModel;

namespace TweetScheduler.Filter
{
    public class UnscheduledTweetFilter : ITweetFilter
    {
        public IEnumerable<TweetViewModel> FilterTweets(IEnumerable<TweetViewModel> tweetsToFilter)
        {
            return tweetsToFilter.Where(t => t.ScheduledDateTime == null && !t.Posted);
        }
    }
}