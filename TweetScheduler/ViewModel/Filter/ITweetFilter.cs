using System.Collections.Generic;
using TweetScheduler.ViewModel;

namespace TweetScheduler.Filter
{
    public interface ITweetFilter
    {
        IEnumerable<TweetViewModel> FilterTweets(IEnumerable<TweetViewModel> tweetsToFilter);
    }
}