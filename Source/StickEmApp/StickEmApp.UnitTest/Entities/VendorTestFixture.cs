using NUnit.Framework;
using StickEmApp.Entities;

namespace StickEmApp.UnitTest.Entities
{
    [TestFixture]
    public class VendorTestFixture
    {
        [TestCase(0, 0, 0)]
        [TestCase(1, 0, 5)]
        [TestCase(5, 0, 25)]
        [TestCase(5, 1, 20)]
        [TestCase(5, 5, 0)]
        public void CalculateTotalAmountRequiredShouldReturnStickerPriceTimesReceivedMinusReturned(int amountReceived, int amountReturned, decimal total)
        {
            var vendor = new Vendor
            {
                NumberOfStickersReceived = amountReceived,
                NumberOfStickersReturned = amountReturned
            };

            Assert.That(vendor.CalculateTotalAmountRequired(), Is.EqualTo(new Money(total)));
        }
    }
}