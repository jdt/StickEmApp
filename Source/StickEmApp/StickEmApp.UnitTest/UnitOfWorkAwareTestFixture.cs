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

        private string DatabaseFile
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "test.db");
            }
        } 
    }
}