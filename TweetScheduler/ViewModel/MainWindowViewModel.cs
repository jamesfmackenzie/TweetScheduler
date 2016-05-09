using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TweetScheduler.Filter;
using TweetScheduler.Model;
using TweetScheduler.Repository;
using TweetScheduler.View.Windows;
using TweetScheduler.Service;

namespace TweetScheduler.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly TweetRepository _tweetRepository;
        private readonly TweetPoster _tweetPoster;

        public MainWindowViewModel()
        {
            // TODO: should initialize in the project root or use DI/IOC
            _tweetRepository = new TweetRepository();
            _tweetPoster = new TweetPoster(_tweetRepository);

            TweetsViewModelCollection = new ObservableCollection<TweetListViewModel>();
            TweetsViewModelCollection.Add(new TweetListViewModel("Unscheduled Tweets", _tweetRepository,
                new UnscheduledTweetFilter()));
            TweetsViewModelCollection.Add(new TweetListViewModel("Scheduled Tweets", _tweetRepository,
                new ScheduledTweetFilter()));
            TweetsViewModelCollection.Add(new TweetListViewModel("Posted Tweets", _tweetRepository,
                new PostedTweetFilter()));

            NewTweetCommand = new RelayCommand(AddTweet);

            DoubleClickTweetCommand = new RelayCommand<TweetViewModel>(EditTweet);
        }

        public ObservableCollection<TweetListViewModel> TweetsViewModelCollection { get; set; }
        public RelayCommand<TweetViewModel> DoubleClickTweetCommand { get; private set; }
        public RelayCommand NewTweetCommand { get; private set; }

        private void EditTweet(TweetViewModel tweetToEdit)
        {
            //TODO: view model should NOT have reference to another view. Use a service instead. http://stackoverflow.com/questions/1043918/open-file-dialog-mvvm/1044304#1044304
            var w = new EditTweetWindow {DataContext = new EditTweetWindowViewModel(tweetToEdit, _tweetRepository)};
            w.ShowDialog();
        }

        private void AddTweet()
        {
            var newTweet = new Tweet();
            EditTweet(new TweetViewModel(newTweet, _tweetRepository));
        }
    }
}