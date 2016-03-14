using System.IO;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using StickEmApp.Dal;

namespace StickEmApp.UnitTest
{
    public class UnitOfWorkAwareTestFixture
    {
        private UnitOfWork _unitOfWork;

        [SetUp]
        public void AwareSetUp()
        {
            UnitOfWorkManager.Initialize(DatabaseFile, DatabaseFileMode.Overwrite);

            _unitOfWork = new UnitOfWork();
        }

        protected ISession Session
        {
            get { return UnitOfWorkManager.Session; }
        }

        [TearDown]
        public void AwareTearDown()
        {
            _unitOfWork.Dispose();
        }

        protected void RenewSession()
        {
            Session.Flush();
            Session.Clear();
        }

        private void ExposeConfiguration(Configuration obj)
        {
            // delete the existing db on each run
            if (File.Exists(DatabaseFile))
                File.Delete(DatabaseFile);

            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(obj)
              .Create(false, true);
        }

        private string DatabaseFile
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "test.db");
            }
        } 
    }
}