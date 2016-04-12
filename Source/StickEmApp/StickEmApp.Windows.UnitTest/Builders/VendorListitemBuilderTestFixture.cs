using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using StickEmApp.Entities;
using StickEmApp.Windows.Builders;

namespace StickEmApp.Windows.UnitTest.Builders
{
    [TestFixture]
    public class VendorListItemBuilderTestFixture
    {
        private VendorListItemBuilder _builder;

        [SetUp]
        public void SetUp()
        {
            _builder = new VendorListItemBuilder();
        }

        [Test]
        public void TestBuild()
        {
            //arrange
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            var working = new TestableVendor
            {
                Id = id1,
                Name = "foo",
                Required = new Money(105),
                Returned = new Money(99),
                Result = new SalesResult {NumberOfStickersReceived = 21, Difference = new Money(6)}
            };

            var finished = new TestableVendor
            {
                Id = id2,
                Name = "bar",
                Required = new Money(55),
                Returned = new Money(55),
                Result = new SalesResult {NumberOfStickersReceived = 11, Difference = new Money(0)}
            };
            finished.Finish(DateTime.Now);

            var input = new List<Vendor> { working, finished };

            //act
            var result = _builder.BuildFrom(input);

            //assert
            Assert.That(result.Count, Is.EqualTo(2));

            var resultItem = result.ElementAt(0);
            Assert.That(resultItem.Id, Is.EqualTo(id1));
            Assert.That(resultItem.Name, Is.EqualTo("foo"));
            Assert.That(resultItem.NumberOfStickersReceived, Is.EqualTo(21));
            Assert.That(resultItem.AmountRequired, Is.EqualTo(new Money(105)));
            Assert.That(resultItem.AmountReturned, Is.EqualTo(new Money(99)));
            Assert.That(resultItem.Difference, Is.EqualTo(new Money(6)));
            Assert.That(resultItem.Status, Is.EqualTo(VendorStatus.Working));

            resultItem = result.ElementAt(1);
            Assert.That(resultItem.Id, Is.EqualTo(id2));
            Assert.That(resultItem.Name, Is.EqualTo("bar"));
            Assert.That(resultItem.NumberOfStickersReceived, Is.EqualTo(11));
            Assert.That(resultItem.AmountRequired, Is.EqualTo(new Money(55)));
            Assert.That(resultItem.AmountReturned, Is.EqualTo(new Money(55)));
            Assert.That(resultItem.Difference, Is.EqualTo(new Money(0)));
            Assert.That(resultItem.Status, Is.EqualTo(VendorStatus.Finished));
        }

        private class TestableVendor : Vendor
        {
            private SalesResult _result;
            public SalesResult Result
            {
                set { _result = value; }
            }

            private Money _required;
            public Money Required
            {
                set { _required = value; }
            }

            private Money _returned;
            public Money Returned
            {
                set { _returned = value; }
            }

            public override Money CalculateTotalAmountRequired()
            {
                return _required;
            }

            public override Money CalculateTotalAmountReturned()
            {
                return _returned;
            }

            public override SalesResult CalculateSalesResult()
            {
                return _result;
            }
        }
    }
}