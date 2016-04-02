using NUnit.Framework;
using StickEmApp.Entities;

namespace StickEmApp.UnitTest.Entities
{
    [TestFixture]
    public class AmountReturnedTestFixture
    {
        [Test]
        public void TestCalculateTotal()
        {
            var amount = new AmountReturned
            {
                FiveHundreds = 2,       //1000.00
                TwoHundreds = 3,        // 600.00
                Hundreds = 4,           // 400.00
                Fifties = 5,            // 250.00
                Twenties = 6,           // 120.00
                Tens = 7,               //  70.00
                Fives = 8,              //  40.00
                Twos = 9,               //  18.00
                Ones = 10,              //  10.00
                FiftyCents = 11,        //   5.50
                TwentyCents = 12,       //   2.40
                TenCents = 13,          //   1.30
                FiveCents = 14,         //   0.70
                TwoCents = 15,          //   0.30
                OneCents = 16           //   0.16
            };

            Assert.That(amount.CalculateTotal(), Is.EqualTo(new Money(2518.36m)));
        }
    }
}