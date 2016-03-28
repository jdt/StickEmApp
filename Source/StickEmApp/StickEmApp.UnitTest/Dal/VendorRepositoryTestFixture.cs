using System.Linq;
using NUnit.Framework;
using StickEmApp.Dal;
using StickEmApp.Entities;

namespace StickEmApp.UnitTest.Dal
{
    [TestFixture]
    public class VendorRepositoryTestFixture : UnitOfWorkAwareTestFixture
    {
        private VendorRepository _repo;

        [SetUp]
        public void SetUp()
        {
            _repo = new VendorRepository();
        }

        [Test]
        public void TestGet()
        {
            var vendor = new Vendor
            {
                Name = "test"
            };

            Session.Save(vendor);

            RenewSession();
            
            var savedVendor = _repo.Get(vendor.Id);

            Assert.That(savedVendor.Name, Is.EqualTo("test"));
        }

        [Test]
        public void TestSave()
        {
            var vendor = new Vendor
            {
                Name = "test"
            };

            _repo.Save(vendor);

            RenewSession();

            var savedVendor = Session.QueryOver<Vendor>().Where(x => x.Name == "test").SingleOrDefault();

            Assert.IsNotNull(savedVendor);
        }

        [Test]
        public void TestSelect()
        {
            var vendor1 = new Vendor {Name = "test 1"};
            var vendor2 = new Vendor {Name = "test 2"};

            var removedTest = new Vendor {Name = "test removed"};
            removedTest.Remove();

            Session.Save(vendor1);
            Session.Save(vendor2);
            Session.Save(removedTest);

            RenewSession();

            var vendors = _repo.SelectVendors().OrderBy(x => x.Name).ToList();

            Assert.That(vendors.Count, Is.EqualTo(2));
            Assert.That(vendors[0].Name, Is.EqualTo("test 1"));
            Assert.That(vendors[1].Name, Is.EqualTo("test 2"));
        }
    }
}
