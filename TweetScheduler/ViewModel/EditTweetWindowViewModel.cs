using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;
using TweetScheduler.Model;
using TweetScheduler.ViewModel.Commands;

namespace TweetScheduler.ViewModel
{
    internal class EditTweetWindowViewModel : INotifyPropertyChanged
    {
        private readonly Tweet _tweet;
        private string _mediaUrls;
        private string _status;
        private int _tweetLength;

        public EditTweetWindowViewModel(Tweet tweet)
        {
            _tweet = tweet;

            Status = tweet.Status;

            if (tweet.MediaUrls != null)
            {
                MediaUrls = string.Join("\n", tweet.MediaUrls);
            }

            ScheduledDateTime = tweet.ScheduledDateTime;
        }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                CalcTweetLength();
            }
        }

        public string MediaUrls
        {
            get { return _mediaUrls; }
            set
            {
                _mediaUrls = value;
                CalcTweetLength();
            }
        }

        public DateTime? ScheduledDateTime { get; set; }

        public int TweetLength
        {
            get { return _tweetLength; }
            set
            {
                _tweetLength = value;
                OnPropertyChanged();
            }
        }

        public ICommand WindowClosing
        {
            get
            {
                return new RelayCommand(
                    args =>
                    {
                        // TODO: verify that all urls are in correct format and eliminate empty strings
                        if (!string.IsNullOrEmpty(MediaUrls))
                        {
                            _tweet.MediaUrls = MediaUrls.Split('\n').ToList();
                        }

                        _tweet.Status = Status;
                        _tweet.ScheduledDateTime = ScheduledDateTime;
                    });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void CalcTweetLength()
        {
            var s = Status;

            if (s != null)
            {
                var usedCharacters = 0;

                var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b",
                    RegexOptions.Compiled | RegexOptions.IgnoreCase);

                foreach (Match m in linkParser.Matches(s))
                {
                    usedCharacters = usedCharacters + 23;
                    s = s.Replace(m.Value, "");
                }

                usedCharacters = usedCharacters + s.Length;

                if (!string.IsNullOrEmpty(MediaUrls))
                {
                    usedCharacters = usedCharacters + 24;
                }

                TweetLength = usedCharacters;
            }

            else
            {
                TweetLength = 0;
            }
        }

        private void OnPropertyChanged([CallerMemberName] String caller = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}