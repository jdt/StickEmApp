using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
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

            var input = new List<Vendor>
            {
                new Vendor {Id = id1, Name = "foo"}, 
                new Vendor {Id = id2, Name = "bar"}
            };

            //act
            var result = _builder.BuildFrom(input);

            //assert
            Assert.That(result.Count, Is.EqualTo(2));

            var resultItem = result.ElementAt(0);
            Assert.That(resultItem.Id, Is.EqualTo(id1));
            Assert.That(resultItem.Name, Is.EqualTo("foo"));

            resultItem = result.ElementAt(1);
            Assert.That(resultItem.Id, Is.EqualTo(id2));
            Assert.That(resultItem.Name, Is.EqualTo("bar"));
        }
    }
}