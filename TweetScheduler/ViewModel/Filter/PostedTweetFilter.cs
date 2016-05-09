using System.Collections.Generic;
using System.Linq;
using TweetScheduler.ViewModel;

namespace TweetScheduler.Filter
{
    public class PostedTweetFilter : ITweetFilter
    {
        public IEnumerable<TweetViewModel> FilterTweets(IEnumerable<TweetViewModel> tweetsToFilter)
        {
            return tweetsToFilter.Where(t => t.Posted);
        }
    }
}