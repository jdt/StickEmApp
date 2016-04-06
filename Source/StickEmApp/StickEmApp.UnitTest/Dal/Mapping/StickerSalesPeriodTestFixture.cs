using FluentNHibernate.Testing;
using NUnit.Framework;
using StickEmApp.Dal;
using StickEmApp.Entities;

namespace StickEmApp.UnitTest.Dal.Mapping
{
    [TestFixture]
    public class StickerSalesPeriodTestFixture : UnitOfWorkAwareTestFixture
    {
        [Test]
        public void TestMap()
        {
            new PersistenceSpecification<StickerSalesPeriod>(UnitOfWorkManager.Session)
            .CheckProperty(c => c.NumberOfStickersToSell, 1337)
            .VerifyTheMappings();
        }
    }
}