using System;
using System.IO;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;

namespace StickEmApp.Dal
{
    internal static class UnitOfWorkManager
    {
        private static ISessionFactory _sessionFactory;

        static UnitOfWorkManager()
        {
            var dbFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "data.db");
            Initialize(dbFile, config => { });
        }

        internal static void Initialize(string databaseFile, Action<Configuration> exposeConfiguration)
        {
            _sessionFactory = Fluently.Configure()
                    .Database(
                        SQLiteConfiguration.Standard
                            .UsingFile(databaseFile)
                    )
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UnitOfWork>())
                    .ExposeConfiguration(exposeConfiguration)
                    .BuildSessionFactory();
        }

        private static ISession _session;

        public static ISession Session
        {
            get
            {
                if (_session == null)
                    throw new InvalidOperationException("There is no active Unit Of Work.");
                return _session;
            }
        }

        public static void Enlist(IUnitOfWork unitOfWork)
        {
            if (_session != null)
                throw new InvalidOperationException("There is already an active Unit Of Work.");
            _session = _sessionFactory.OpenSession();
        }

        public static void Release()
        {
            if (_session == null)
                throw new InvalidOperationException("There is no active Unit Of Work to release.");

            _session.Flush();
            _session.Dispose();
            _session = null;
        }
    }
}
