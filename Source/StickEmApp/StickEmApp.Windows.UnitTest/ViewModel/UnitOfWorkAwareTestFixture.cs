using NHibernate;
using NUnit.Framework;
using Rhino.Mocks;
using StickEmApp.Dal;

namespace StickEmApp.Windows.UnitTest.ViewModel
{
    public class UnitOfWorkAwareTestFixture
    {
        [SetUp]
        public void SetUpUnitOfWorkAwareTestFixture()
        {
            var mockSessionFactory = MockRepository.GenerateMock<ISessionFactory>();

            var mockSession = MockRepository.GenerateMock<ISession>();
            mockSessionFactory.Expect(p => p.OpenSession()).Return(mockSession);

            UnitOfWorkManager.Initialize(mockSessionFactory);
        }
    }
}