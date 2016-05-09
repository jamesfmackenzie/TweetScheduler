using System;
using System.Linq;
using System.Text.RegularExpressions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TweetScheduler.Model;
using TweetScheduler.Repository;

namespace TweetScheduler.ViewModel
{
    internal class EditTweetWindowViewModel : ObservableObject
    {
        private readonly TweetViewModel _tweetViewModel;
        private readonly TweetRepository _tweetRepository;
        private string _mediaUrls;
        private string _status;
        private int _tweetLength;

        public EditTweetWindowViewModel(TweetViewModel tweetViewModel, TweetRepository tweetRepository)
        {
            _tweetViewModel = tweetViewModel;
            _tweetRepository = tweetRepository;

            Status = tweetViewModel.Status;

            if (tweetViewModel.MediaUrls != null)
            {
                MediaUrls = string.Join("\n", tweetViewModel.MediaUrls);
            }

            ScheduledDateTime = tweetViewModel.ScheduledDateTime;

            WindowClosingCommand = new RelayCommand(SaveTweetAndCloseWindow);
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
            set { Set(() => TweetLength, ref _tweetLength, value); }
        }

        public RelayCommand WindowClosingCommand { get; private set; }

        private void SaveTweetAndCloseWindow()
        {
            // TODO: validation?

            if (!string.IsNullOrEmpty(MediaUrls))
            {
                _tweetViewModel.MediaUrls = MediaUrls.Replace("\r", "").Split('\n').ToList();
            }

            _tweetViewModel.Status = Status;
            _tweetViewModel.ScheduledDateTime = ScheduledDateTime;
            _tweetViewModel.Posted = false;

            _tweetRepository.SaveOrUpdate(new Tweet(_tweetViewModel));
        }

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
    }
}