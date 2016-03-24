using System;
using System.Collections.Generic;
using NUnit.Framework;
using Prism.Events;
using Prism.Regions;
using Rhino.Mocks;
using StickEmApp.Dal;
using StickEmApp.Entities;
using StickEmApp.Windows.Builders;
using StickEmApp.Windows.Infrastructure;
using StickEmApp.Windows.Infrastructure.Events;
using StickEmApp.Windows.ViewModel;

namespace StickEmApp.Windows.UnitTest.ViewModel
{
    [TestFixture]
    public class VendorListViewModelTestFixture : UnitOfWorkAwareTestFixture
    {
        private IVendorRepository _vendorRepository;
        private IVendorListItemBuilder _vendorListItemBuilder;
        private IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;

        private VendorUpdatedEvent _vendorUpdatedEvent;

        private VendorListViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _vendorRepository = MockRepository.GenerateMock<IVendorRepository>();
            _regionManager = MockRepository.GenerateMock<IRegionManager>();

            _vendorListItemBuilder = MockRepository.GenerateMock<IVendorListItemBuilder>();

            _eventAggregator = MockRepository.GenerateMock<IEventAggregator>();
            _vendorUpdatedEvent = MockRepository.GenerateMock<VendorUpdatedEvent>();
            _eventAggregator.Expect(p => p.GetEvent<VendorUpdatedEvent>()).Return(_vendorUpdatedEvent);
        }

        [Test]
        public void VendorListViewModelShouldLoadVendorsInVendorList()
        {
            //arrange
            var vendorList = new List<Vendor> { new Vendor() };
            _vendorRepository.Expect(p => p.SelectVendors()).Return(vendorList);

            var viewModelItem = new VendorListItem(Guid.NewGuid(), "test1");
            var viewModelList = new List<VendorListItem> { viewModelItem };
            _vendorListItemBuilder.Expect(p => p.BuildFrom(vendorList)).Return(viewModelList);

            //act
            _viewModel = new VendorListViewModel(_vendorRepository, _vendorListItemBuilder, _regionManager, _eventAggregator);

            //assert
            var vendors = _viewModel.VendorList;

            Assert.That(vendors.Count, Is.EqualTo(1));
            Assert.That(vendors[0], Is.EqualTo(viewModelItem));
        }

        [Test]
        public void AddVendorCommandShouldNavigateToVendorDetailView()
        {
            _vendorListItemBuilder.Expect(b => b.BuildFrom(null)).Return(new List<VendorListItem>());
            _viewModel = new VendorListViewModel(_vendorRepository, _vendorListItemBuilder, _regionManager, _eventAggregator);

            _regionManager.Expect(rm => rm.RequestNavigate(RegionNames.EditVendorRegion, new Uri("VendorDetailView", UriKind.Relative)));

            //act
            _viewModel.AddVendorCommand.Execute(null);

            //assert
            _regionManager.VerifyAllExpectations();
        }
    }
}