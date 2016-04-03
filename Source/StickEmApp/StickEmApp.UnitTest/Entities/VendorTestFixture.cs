using NUnit.Framework;
using StickEmApp.Entities;

namespace StickEmApp.UnitTest.Entities
{
    [TestFixture]
    public class VendorTestFixture
    {
        [TestCase(0, 0, 0, 0)]
        [TestCase(1, 0, 0, 5)]
        [TestCase(5, 0, 0, 25)]
        [TestCase(5, 1, 0, 20)]
        [TestCase(5, 5, 0, 0)]
        [TestCase(5, 5, 50, 50)]
        [TestCase(10, 0, 50, 100)]
        public void CalculateTotalAmountRequiredShouldReturnTotalAmountRequiredToBePaid(int amountReceived, int amountReturned, int changeReceived, decimal total)
        {
            var vendor = new Vendor
            {
                NumberOfStickersReceived = amountReceived,
                NumberOfStickersReturned = amountReturned,
                ChangeReceived = new Money(changeReceived)
            };

            Assert.That(vendor.CalculateTotalAmountRequired(), Is.EqualTo(new Money(total)));
        }

        [Test]
        public void CalculateSalesResultShouldReturnExactIfRequiredAmountEqualsReturnedAmount()
        {
            var vendor = new Vendor
            {
                NumberOfStickersReceived = 5,
                AmountReturned = new AmountReturned
                {
                    Fives = 5
                }
            };

            var result = vendor.CalculateSalesResult();
            Assert.That(result.Status, Is.EqualTo(ResultType.Exact));
            Assert.That(result.Difference, Is.EqualTo(new Money(0)));
        }

        [Test]
        public void CalculateSalesResultShouldReturnShortageIfRequiredAmountIsMoreThanReturnedAmount()
        {
            var vendor = new Vendor
            {
                NumberOfStickersReceived = 5,
                AmountReturned = new AmountReturned
                {
                    Fives = 4
                }
            };

            var result = vendor.CalculateSalesResult();
            Assert.That(result.Status, Is.EqualTo(ResultType.Shortage));
            Assert.That(result.Difference, Is.EqualTo(new Money(5)));
        }

        [Test]
        public void CalculateSalesResultShouldReturnSurplusIfRequiredAmountIsLessThanReturnedAmount()
        {
            var vendor = new Vendor
            {
                NumberOfStickersReceived = 5,
                AmountReturned = new AmountReturned
                {
                    Fives = 6
                }
            };

            var result = vendor.CalculateSalesResult();
            Assert.That(result.Status, Is.EqualTo(ResultType.Surplus));
            Assert.That(result.Difference, Is.EqualTo(new Money(5)));
        }
    }
}