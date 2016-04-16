using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TweetScheduler.Model
{
    // TODO: should *NOT* implement INotifyPropertyChanged (separate viewmodel class for that!)
    internal class Tweet : INotifyPropertyChanged
    {
        private List<string> _mediaUrls;
        private bool _posted;
        private DateTime? _scheduledDateTime;
        private string _status;

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public DateTime? ScheduledDateTime
        {
            get { return _scheduledDateTime; }
            set
            {
                _scheduledDateTime = value;
                OnPropertyChanged();
            }
        }

        public List<string> MediaUrls
        {
            get { return _mediaUrls; }
            set
            {
                _mediaUrls = value;
                OnPropertyChanged();
            }
        }

        public bool Posted
        {
            get { return _posted; }
            set
            {
                _posted = value;
                OnPropertyChanged();
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
    }
}