using System;
using System.IO;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace StickEmApp.Dal
{
    public static class UnitOfWorkManager
    {
        public static void Initialize(string databaseFile, DatabaseFileMode mode)
        {
            _sessionFactory = Fluently.Configure()
                    .Database(
                        SQLiteConfiguration.Standard
                            .UsingFile(databaseFile)
                    )
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UnitOfWork>())
                    .ExposeConfiguration((configuration =>
                    {
                        if (mode == DatabaseFileMode.Overwrite && File.Exists(databaseFile))
                        {
                            File.Delete(databaseFile);
                        }

                        if (File.Exists(databaseFile) == false)
                        {
                            // this NHibernate tool takes a configuration (with mapping info in)
                            // and exports a database schema from it
                            new SchemaExport(configuration).Create(false, true);
                        }
                    }))
                    .BuildSessionFactory();
        }

        public static void Initialize(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        private static ISessionFactory _sessionFactory;
        private static ISessionFactory SessionFactory
        {
            get
            {
                if(_sessionFactory == null)
                    throw new InvalidOperationException("The UnitOfWorkManager was not initialized.");

                return _sessionFactory;
            }
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
            _session = SessionFactory.OpenSession();
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
