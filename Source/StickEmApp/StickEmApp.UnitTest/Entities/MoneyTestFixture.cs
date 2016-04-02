using NUnit.Framework;
using StickEmApp.Entities;

namespace StickEmApp.UnitTest.Entities
{
    [TestFixture]
    public class MoneyTestFixture
    {
        [TestCase(0, 0, true)]
        [TestCase(5.5, 5.5, true)]
        [TestCase(1, 0, false)]
        [TestCase(1.1, 3, false)]
        public void TestEquals(decimal a, decimal b, bool expected)
        {
            var money1 = new Money(a);
            var money2 = new Money(b);

            Assert.That(money1.Equals(money2), Is.EqualTo(expected));
            Assert.That(money2.Equals(money1), Is.EqualTo(expected));
        }

        [TestCase(0, 0, 0)]
        [TestCase(0, 3, 0)]
        [TestCase(1, 5, 5)]
        [TestCase(3, 5, 15)]
        [TestCase(2, 5.5, 11)]
        public void TestMultiplication(int times, decimal money, decimal expected)
        {
            var result = times*new Money(money);
            Assert.That(result, Is.EqualTo(new Money(expected)));
        }

        [TestCase(0, 0, 0)]
        [TestCase(0, 5, -5)]
        [TestCase(5, 5, 0)]
        [TestCase(5.5, 5, 0.5)]
        [TestCase(5, 2, 3)]
        public void TestSubtraction(decimal a, decimal b, decimal expected)
        {
            var result = new Money(a) - new Money(b);
            Assert.That(result, Is.EqualTo(new Money(expected)));
        }
    }
}
