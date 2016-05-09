using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using TweetScheduler.Filter;
using TweetScheduler.Repository;

namespace TweetScheduler.ViewModel
{
    internal class TweetListViewModel : ObservableObject
    {
        private readonly ITweetFilter _filter;
        private readonly TweetRepository _tweetRepository;
        private string _name;
        private ObservableCollection<TweetViewModel> _tweets;

        public TweetListViewModel(string name, TweetRepository tweetRepository, ITweetFilter filter)
        {
            _tweetRepository = tweetRepository;
            _filter = filter;
            Name = name;
            Tweets = new ObservableCollection<TweetViewModel>(_tweetRepository.GetAll().Select(t => new TweetViewModel(t, _tweetRepository)).OrderBy(t => t.ScheduledDateTime));

            tweetRepository.TweetAddedUpdatedOrDeleted += TweetRepository_TweetAddedUpdatedOrDeleted;
        }

        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }

        public ObservableCollection<TweetViewModel> Tweets
        {
            get { return new ObservableCollection<TweetViewModel>(_filter.FilterTweets(_tweets)); }
            set { Set(() => Tweets, ref _tweets, value); }
        }

        private void TweetRepository_TweetAddedUpdatedOrDeleted(object sender, EventArgs e)
        {
            Tweets = new ObservableCollection<TweetViewModel>(_tweetRepository.GetAll().Select(t => new TweetViewModel(t, _tweetRepository)).OrderBy(t => t.ScheduledDateTime));
        }
    }
}