using System.Collections.Generic;

namespace TweetScheduler.ViewModel.Filter
{
    public interface ITweetFilter
    {
        IEnumerable<TweetViewModel> FilterTweets(IEnumerable<TweetViewModel> tweetsToFilter);
    }
}