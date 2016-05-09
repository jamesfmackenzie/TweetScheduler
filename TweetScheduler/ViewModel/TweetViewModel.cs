using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GalaSoft.MvvmLight.Command;
using TweetScheduler.Model;
using TweetScheduler.Repository;
using TweetScheduler.View.Windows;

namespace TweetScheduler.ViewModel
{
    public class TweetViewModel
    {
        private readonly TweetRepository _tweetRepository;

        public TweetViewModel(Tweet tweet, TweetRepository tweetRepository)
        {
            Id = tweet.Id;
            Status = tweet.Status;
            ScheduledDateTime = tweet.ScheduledDateTime;
            MediaUrls = tweet.MediaUrls;
            Posted = tweet.Posted;

            _tweetRepository = tweetRepository;

            EditButtonClickCommand = new RelayCommand<TweetViewModel>(EditTweet);
            DeleteButtonClickCommand = new RelayCommand<TweetViewModel>(DeleteTweet);
        }

        public RelayCommand<TweetViewModel> EditButtonClickCommand { get; private set; }
        public RelayCommand<TweetViewModel> DeleteButtonClickCommand { get; private set; }
        public string Status { get; set; }
        public Guid Id { get; set; }

        public string StatusWithoutUrl
        {
            get
            {
                var toReturn = Status;

                if (Status != null)
                {
                    var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    foreach (Match m in linkParser.Matches(toReturn))
                    {
                        toReturn = toReturn.Replace(m.Value, "");
                    }
                }

                return toReturn;
            }
        }

        public List<string> StatusUrls
        {
            get
            {
                var toReturn = new List<string>();

                if (Status != null)
                {
                    var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    foreach (Match m in linkParser.Matches(Status))
                    {
                        toReturn.Add(m.Value);
                    }
                }

                return toReturn;
            }
        }

        public DateTime? ScheduledDateTime { get; set; }
        public List<string> MediaUrls { get; set; }
        public bool Posted { get; set; }

        private void EditTweet(TweetViewModel tweetToEdit)
        {
            //TODO: view model should NOT have reference to another view. Use a service instead. http://stackoverflow.com/questions/1043918/open-file-dialog-mvvm/1044304#1044304
            var w = new EditTweetWindow {DataContext = new EditTweetWindowViewModel(tweetToEdit, _tweetRepository)};
            w.ShowDialog();
        }

        private void DeleteTweet(TweetViewModel tweetToDelete)
        {
            _tweetRepository.Delete(new Tweet(tweetToDelete));
        }
    }
}