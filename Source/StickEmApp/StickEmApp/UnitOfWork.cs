using System;
using StickEmApp.Dal;

namespace StickEmApp
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public UnitOfWork()
        {
            UnitOfWorkManager.Enlist(this);
        }

        public void Dispose()
        {
            UnitOfWorkManager.Release();
        }

        public T Get<T>(Guid id)
        {
            return UnitOfWorkManager.Session.Get<T>(id);
        }

        public void SaveOrUpdate<T>(T obj)
        {
            UnitOfWorkManager.Session.SaveOrUpdate(obj);
        }
    }
}