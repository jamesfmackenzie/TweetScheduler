using System;
using System.Collections.Generic;
using TweetScheduler.ViewModel;

namespace TweetScheduler.Model
{
    public class Tweet
    {
        public Tweet()
        {
            Id = Guid.NewGuid();
        }

        // TODO: should this translation be here? It builds dependency between Model and ViewModel
        public Tweet(TweetViewModel tweetViewModel)
        {
            Status = tweetViewModel.Status;
            Id = tweetViewModel.Id;
            ScheduledDateTime = tweetViewModel.ScheduledDateTime;
            MediaUrls = tweetViewModel.MediaUrls;
            Posted = tweetViewModel.Posted;
        }

        public string Status { get; set; }
        public Guid Id { get; set; }
        public DateTime? ScheduledDateTime { get; set; }
        public List<string> MediaUrls { get; set; }
        public bool Posted { get; set; }
    }
}