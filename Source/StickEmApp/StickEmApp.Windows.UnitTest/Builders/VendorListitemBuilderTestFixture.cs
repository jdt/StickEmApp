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
            var input = new List<Vendor>
            {
                new Vendor {Name = "foo"}, 
                new Vendor {Name = "bar"}
            };

            //act
            var result = _builder.BuildFrom(input);

            //assert
            Assert.That(result.Count, Is.EqualTo(2));

            var resultItem = result.ElementAt(0);
            Assert.That(resultItem.Name, Is.EqualTo("foo"));

            resultItem = result.ElementAt(1);
            Assert.That(resultItem.Name, Is.EqualTo("bar"));
        }
    }
}