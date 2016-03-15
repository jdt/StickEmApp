using NUnit.Framework;
using StickEmApp.Entities;
using StickEmApp.Windows.ViewModel;

namespace StickEmApp.Windows.UnitTest.ViewModel
{
    [TestFixture]
    public class ViewModelFactoryTestFixture
    {
        private ViewModelFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new ViewModelFactory();
        }

        [Test]
        public void TestVendorViewModel()
        {
            //arrange
            var vendor = new Vendor
            {
                Name = "Vendor 1"
            };

            //act
            var viewModel = _factory.VendorViewModel(vendor);

            //assert
            Assert.That(viewModel.Name, Is.EqualTo("Vendor 1"));
        }
    }
}