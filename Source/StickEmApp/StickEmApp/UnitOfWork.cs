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
    }
}