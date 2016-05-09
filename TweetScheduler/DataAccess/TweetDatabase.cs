using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TweetScheduler.Model;

namespace TweetScheduler.DataAccess
{
    internal class TweetDatabase
    {
        private readonly string _databaseFile;
        private readonly object _fileLock;
        private readonly List<Tweet> _tweets;
        private readonly XmlSerializer _xmlSerializer;

        public TweetDatabase()
        {
            _tweets = new List<Tweet>();
            _xmlSerializer = new XmlSerializer(_tweets.GetType());
            _databaseFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                            "//TweetScheduler.xml";
            _fileLock = new object();

            lock (_fileLock)
            {
                if (File.Exists(_databaseFile))
                {
                    var streamReader = new StreamReader(_databaseFile);
                    _tweets = (List<Tweet>) _xmlSerializer.Deserialize(streamReader);
                    streamReader.Close();
                }
            }
        }

        public List<Tweet> RetrieveAll()
        {
            return _tweets;
        }

        public void DeleteById(Guid id)
        {
            _tweets.RemoveAll(t => t.Id == id);
            FlushDatabaseToDisk();
        }

        public Tweet RetrieveById(Guid id)
        {
            return _tweets.Find(t => t.Id == id);
        }

        public void Create(Tweet tweetToCreate)
        {
            _tweets.Add(tweetToCreate);
            FlushDatabaseToDisk();
        }

        private void FlushDatabaseToDisk()
        {
            lock (_fileLock)
            {
                if (File.Exists(_databaseFile))
                {
                    File.Delete(_databaseFile);
                }

                var stream = File.Create(_databaseFile);
                _xmlSerializer.Serialize(stream, _tweets);
                stream.Close();
            }
        }
    }
}