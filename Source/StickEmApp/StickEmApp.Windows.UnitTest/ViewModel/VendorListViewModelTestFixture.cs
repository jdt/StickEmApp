using System.Collections.Generic;
using NUnit.Framework;
using Prism.Events;
using Prism.Regions;
using Rhino.Mocks;
using StickEmApp.Dal;
using StickEmApp.Entities;
using StickEmApp.Windows.ViewModel;

namespace StickEmApp.Windows.UnitTest.ViewModel
{
    [TestFixture]
    public class VendorListViewModelTestFixture : UnitOfWorkAwareTestFixture
    {
        private IVendorRepository _vendorRepository;
        private IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;

        private VendorListViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _vendorRepository = MockRepository.GenerateMock<IVendorRepository>();
            _regionManager = MockRepository.GenerateMock<IRegionManager>();
            _eventAggregator = MockRepository.GenerateMock<IEventAggregator>();
        }

        [Test]
        public void VendorListViewModelShouldLoadVendorsInVendorList()
        {
            //arrange
            var vendor1 = new Vendor();
            var vendor2 = new Vendor();
            _vendorRepository.Expect(p => p.SelectVendors()).Return(new List<Vendor> {vendor1, vendor2});

            var viewModel1 = new VendorListitem("test1");
            var viewModel2 = new VendorListitem("test2");
            
            //act
            _viewModel = new VendorListViewModel(_vendorRepository, _regionManager, _eventAggregator);

            //assert
            var vendors = _viewModel.VendorList;

            Assert.That(vendors.Count, Is.EqualTo(2));
            Assert.That(vendors[0], Is.EqualTo(viewModel1));
            Assert.That(vendors[1], Is.EqualTo(viewModel2));
        }
    }
}