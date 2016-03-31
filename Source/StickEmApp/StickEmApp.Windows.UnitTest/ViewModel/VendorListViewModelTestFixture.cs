using System;
using System.Collections.Generic;
using NUnit.Framework;
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
        private IEventBus _eventBus;

        private VendorListViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _vendorRepository = MockRepository.GenerateMock<IVendorRepository>();
            _vendorListItemBuilder = MockRepository.GenerateMock<IVendorListItemBuilder>();
            _regionManager = MockRepository.GenerateMock<IRegionManager>();
            _eventBus = MockRepository.GenerateMock<IEventBus>();

            _viewModel = new VendorListViewModel(_vendorRepository, _vendorListItemBuilder, _regionManager, _eventBus);
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
            var vendors = _viewModel.VendorList;
            
            //assert
            Assert.That(vendors.Count, Is.EqualTo(1));
            Assert.That(vendors[0], Is.EqualTo(viewModelItem));
        }
        
        [Test]
        public void AddVendorCommandShouldNavigateToVendorDetailView()
        {
            _regionManager.Expect(rm => rm.RequestNavigate(RegionNames.EditVendorRegion, new Uri("VendorDetailView", UriKind.Relative)));

            //act
            _viewModel.AddVendorCommand.Execute(null);

            //assert
            _regionManager.VerifyAllExpectations();
        }

        [Test]
        public void RemoveVendorCommandShouldRemoveVendorAndRaiseVendorRemovedEvent()
        {
            var removedVendorId = Guid.NewGuid();
            var removedVendor = new Vendor
            {
                Id = removedVendorId
            };
            
            _vendorRepository.Expect(p => p.Get(removedVendorId)).Return(removedVendor);
            _eventBus.Expect(ea => ea.Publish<VendorRemovedEvent, Guid>(removedVendorId));
            
            var removedItem = new VendorListItem(removedVendorId, "test");
            
            //act
            _viewModel.RemoveVendorCommand.Execute(removedItem);

            //assert
            _regionManager.VerifyAllExpectations();
            _eventBus.VerifyAllExpectations();
        }
    }

    [TestFixture]
    public class VendorListViewModelEventSubscriptionTestFixture : UnitOfWorkAwareTestFixture
    {
        private IVendorRepository _vendorRepository;
        private IVendorListItemBuilder _vendorListItemBuilder;
        private IRegionManager _regionManager;
        private IEventBus _eventBus;

        private VendorListViewModel _viewModel;
        private VendorListItem _updatedViewModelItem;

        [SetUp]
        public void SetUp()
        {
            _vendorRepository = MockRepository.GenerateMock<IVendorRepository>();
            _vendorListItemBuilder = MockRepository.GenerateMock<IVendorListItemBuilder>();
            _regionManager = MockRepository.GenerateMock<IRegionManager>();
            _eventBus = MockRepository.GenerateMock<IEventBus>();

            _viewModel = new VendorListViewModel(_vendorRepository, _vendorListItemBuilder, _regionManager, _eventBus);

            var updatedVendorList = new List<Vendor> { new Vendor() };
            _vendorRepository.Expect(p => p.SelectVendors()).Return(updatedVendorList);

            _updatedViewModelItem = new VendorListItem(Guid.NewGuid(), "test2");
            var updatedViewModelList = new List<VendorListItem> { _updatedViewModelItem };
            _vendorListItemBuilder.Expect(p => p.BuildFrom(updatedVendorList)).Return(updatedViewModelList);
        }
        
        [Test]
        public void VendorUpdatedEventShouldReloadVendorData()
        {
            //arrange
            Action<Guid> callback = null;
            _eventBus.Expect(
                p =>
                    p.On<VendorUpdatedEvent, Guid>(Arg<Action<Guid>>.Is.Anything))
                .WhenCalled(cb => callback = (Action<Guid>)cb.Arguments[0]);

            _viewModel = new VendorListViewModel(_vendorRepository, _vendorListItemBuilder, _regionManager, _eventBus);

            //act
            callback(Guid.NewGuid());

            //assert
            AssertDataLoaded();
        }

        [Test]
        public void VendorRemovedEventShouldReloadVendorData()
        {
            //arrange
            Action<Guid> callback = null;
            _eventBus.Expect(
                p =>
                    p.On<VendorRemovedEvent, Guid>(Arg<Action<Guid>>.Is.Anything))
                .WhenCalled(cb => callback = (Action<Guid>)cb.Arguments[0]);

            _viewModel = new VendorListViewModel(_vendorRepository, _vendorListItemBuilder, _regionManager, _eventBus);

            //act
            callback(Guid.NewGuid());

            //assert
            AssertDataLoaded();
        }

        private void AssertDataLoaded()
        {
            var vendors = _viewModel.VendorList;

            Assert.That(vendors.Count, Is.EqualTo(1));
            Assert.That(vendors[0], Is.EqualTo(_updatedViewModelItem));
        }
    }
}