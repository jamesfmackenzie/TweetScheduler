using System.Collections.Generic;

namespace TweetScheduler.Repository
{
    internal interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T SaveOrUpdate(T entity);
        void Delete(T entity);
    }
}