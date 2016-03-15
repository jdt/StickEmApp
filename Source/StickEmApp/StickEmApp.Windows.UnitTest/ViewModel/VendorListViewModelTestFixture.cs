using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using StickEmApp.Dal;
using StickEmApp.Entities;
using StickEmApp.Windows.ViewModel;

namespace StickEmApp.Windows.UnitTest.ViewModel
{
    [TestFixture]
    public class VendorListViewModelTestFixture : UnitOfWorkAwareTestFixture
    {
        private IViewModelFactory _viewModelFactory;
        private IVendorRepository _vendorRepository;

        private VendorListViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _viewModelFactory = MockRepository.GenerateMock<IViewModelFactory>();
            _vendorRepository = MockRepository.GenerateMock<IVendorRepository>();
        }

        [Test]
        public void VendorListViewModelShouldLoadVendorsInVendorList()
        {
            //arrange
            var vendor1 = new Vendor();
            var vendor2 = new Vendor();
            _vendorRepository.Expect(p => p.SelectVendors()).Return(new List<Vendor> {vendor1, vendor2});

            var viewModel1 = new VendorViewModel("test1");
            var viewModel2 = new VendorViewModel("test2");
            _viewModelFactory.Expect(p => p.VendorViewModel(vendor1)).Return(viewModel1);
            _viewModelFactory.Expect(p => p.VendorViewModel(vendor2)).Return(viewModel2);

            //act
            _viewModel = new VendorListViewModel(_viewModelFactory, _vendorRepository);

            //assert
            var vendors = _viewModel.VendorList;

            Assert.That(vendors.Count, Is.EqualTo(2));
            Assert.That(vendors[0], Is.EqualTo(viewModel1));
            Assert.That(vendors[1], Is.EqualTo(viewModel2));
        }
    }
}