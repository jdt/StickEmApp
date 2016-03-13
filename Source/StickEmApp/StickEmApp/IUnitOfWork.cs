using System;

namespace StickEmApp
{
    public interface IUnitOfWork
    {
        T Get<T>(Guid id);
        void SaveOrUpdate<T>(T obj);
    }
}