using System.Collections.Generic;
using System.Linq;

namespace TweetScheduler.ViewModel.Filter
{
    public class UnscheduledTweetFilter : ITweetFilter
    {
        public IEnumerable<TweetViewModel> FilterTweets(IEnumerable<TweetViewModel> tweetsToFilter)
        {
            return tweetsToFilter.Where(t => t.ScheduledDateTime == null && !t.Posted);
        }
    }
}