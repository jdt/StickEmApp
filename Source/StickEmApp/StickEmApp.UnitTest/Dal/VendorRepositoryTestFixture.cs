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

            Session.Flush();

            var savedVendor = Session.QueryOver<Vendor>().Where(x => x.Name == "test").SingleOrDefault();

            Assert.IsNotNull(savedVendor);
        }
    }
}
