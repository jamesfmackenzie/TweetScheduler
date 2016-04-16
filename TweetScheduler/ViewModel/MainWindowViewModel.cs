using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.Threading;
using TweetScheduler.Model;
using TweetScheduler.View.Windows;
using TweetScheduler.ViewModel.Collections;
using TweetScheduler.ViewModel.Commands;

// TODO: fix imports

namespace TweetScheduler.ViewModel
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly TweetDatabase _database = new TweetDatabase();
        private ICommand _newTweetCommand;
        private TrulyObservableCollection<Tweet> _postedTweets;
        // TODO: binds directly to model. Should use a ViewModel instead.
        private TrulyObservableCollection<Tweet> _scheduledTweets;
        private TweetPoster _tweetPoster;
        private TrulyObservableCollection<Tweet> _unscheduledTweets;

        public MainWindowViewModel()
        {
            DoubleClickTweetCommand = new RelayCommand(ShowMessage);

            // TODO: should it b here?
            DispatcherHelper.Initialize();
        }

        public ICommand DoubleClickTweetCommand { get; set; }
        // TODO: binds directly to model. Should use a ViewModel instead.
        public TrulyObservableCollection<Tweet> UnscheduledTweets
        {
            get
            {
                if (_unscheduledTweets == null)
                {
                    _unscheduledTweets = new TrulyObservableCollection<Tweet>(_database.UnscheduledTweets);
                    _unscheduledTweets.CollectionChanged += on_tweetCollectionChanged;
                }
                return _unscheduledTweets;
            }
            set
            {
                _unscheduledTweets = value;
                OnPropertyChanged();
            }
        }

        // TODO: binds directly to model. Should use a ViewModel instead.
        public TrulyObservableCollection<Tweet> PostedTweets
        {
            get
            {
                if (_postedTweets == null)
                {
                    _postedTweets = new TrulyObservableCollection<Tweet>(_database.PostedTweets);
                    _postedTweets.CollectionChanged += on_tweetCollectionChanged;
                }
                return _postedTweets;
            }
            set
            {
                _postedTweets = value;
                OnPropertyChanged();
            }
        }

        public TrulyObservableCollection<Tweet> ScheduledTweets
        {
            get
            {
                if (_scheduledTweets == null)
                {
                    _scheduledTweets = new TrulyObservableCollection<Tweet>(_database.ScheduledTweets);
                    _scheduledTweets.CollectionChanged += on_tweetCollectionChanged;
                    _tweetPoster = new TweetPoster(_scheduledTweets.ToList(), this);
                }
                return _scheduledTweets;
            }
            set
            {
                _scheduledTweets = value;
                OnPropertyChanged();
                _tweetPoster.Tweets = _scheduledTweets.ToList();
            }
        }

        public string AccessKey
        {
            get { return "foo"; }
            set { }
        }

        public string AccessToken
        {
            get { return "foo"; }
            set { }
        }

        public string ConsumerKey
        {
            get { return "foo"; }
            set { }
        }

        public string ConsumerToken
        {
            get { return "foo"; }
            set { }
        }

        public ICommand NewTweetCommand
        {
            get
            {
                if (_newTweetCommand == null)
                {
                    _newTweetCommand = new RelayCommand(
                        param => NewTweet()
                        );
                }
                return _newTweetCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] String caller = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(caller));
            }
        }

        public void on_tweetCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ScheduledTweets = new TrulyObservableCollection<Tweet>(_database.ScheduledTweets);
            UnscheduledTweets = new TrulyObservableCollection<Tweet>(_database.UnscheduledTweets);
            PostedTweets = new TrulyObservableCollection<Tweet>(_database.PostedTweets);
        }

        // TODO: wrong name 
        public void ShowMessage(object obj)
        {
            var tweetToEdit = (Tweet) obj;

            //TODO: view model should NOT have reference to another view. Use a service instead. http://stackoverflow.com/questions/1043918/open-file-dialog-mvvm/1044304#1044304
            var w = new EditTweetWindow();

            w.DataContext = new EditTweetWindowViewModel(tweetToEdit);

            w.ShowDialog();
        }

        public void NewTweet()
        {
            var tweetToCreate = new Tweet();

            // TODO: should probably add this after, not before (so it can be cancelled)
            _database.AddTweet(tweetToCreate);

            // TODO: remove hack
            on_tweetCollectionChanged(null, null);

            //TODO: view model should NOT have reference to another view. Use a service instead. http://stackoverflow.com/questions/1043918/open-file-dialog-mvvm/1044304#1044304
            var w = new EditTweetWindow();

            w.DataContext = new EditTweetWindowViewModel(tweetToCreate);

            w.ShowDialog();

            // TODO: remove hack
            on_tweetCollectionChanged(null, null);
        }
    }
}