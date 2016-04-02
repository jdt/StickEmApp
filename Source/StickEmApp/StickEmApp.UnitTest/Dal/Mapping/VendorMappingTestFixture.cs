using FluentNHibernate.Testing;
using NUnit.Framework;
using StickEmApp.Dal;
using StickEmApp.Entities;

namespace StickEmApp.UnitTest.Dal.Mapping
{
    [TestFixture]
    public class VendorMappingTestFixture : UnitOfWorkAwareTestFixture
    {
        [Test]
        public void TestMap()
        {
            new PersistenceSpecification<Vendor>(UnitOfWorkManager.Session)
            .CheckProperty(c => c.Name, "foo")
            .CheckProperty(c => c.ChangeReceived, new Money(55))
            .CheckProperty(c => c.NumberOfStickersReceived, 10)
            .CheckProperty(c => c.NumberOfStickersReturned, 5)
            .CheckProperty(c => c.AmountReturned.FiveHundreds, 1)
            .CheckProperty(c => c.AmountReturned.TwoHundreds, 1)
            .CheckProperty(c => c.AmountReturned.Hundreds, 1)
            .CheckProperty(c => c.AmountReturned.Fifties, 1)
            .CheckProperty(c => c.AmountReturned.Twenties, 1)
            .CheckProperty(c => c.AmountReturned.Tens, 1)
            .CheckProperty(c => c.AmountReturned.Fives, 1)
            .CheckProperty(c => c.AmountReturned.Twos, 1)
            .CheckProperty(c => c.AmountReturned.Ones, 1)
            .CheckProperty(c => c.AmountReturned.FiftyCents, 1)
            .CheckProperty(c => c.AmountReturned.TwentyCents, 1)
            .CheckProperty(c => c.AmountReturned.TenCents, 1)
            .CheckProperty(c => c.AmountReturned.FiveCents, 1)
            .CheckProperty(c => c.AmountReturned.TwoCents, 1)
            .CheckProperty(c => c.AmountReturned.OneCents, 1)
            .CheckProperty(c => c.Status, VendorStatus.Removed)
            .VerifyTheMappings();
        }
    }
}