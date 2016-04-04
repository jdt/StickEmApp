﻿using System;
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
        public void AddVendorCommandShouldNavigateToVendorDetailViewAndDisallowAddEditRemove()
        {
            _regionManager.Expect(rm => rm.RequestNavigate(RegionNames.EditVendorRegion, new Uri("VendorDetailView", UriKind.Relative)));

            //act
            _viewModel.AddVendorCommand.Execute();

            //assert
            _regionManager.VerifyAllExpectations();
            Assert.That(_viewModel.AddVendorCommand.CanExecute(), Is.False);
            Assert.That(_viewModel.EditVendorCommand.CanExecute(null), Is.False);
            Assert.That(_viewModel.RemoveVendorCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void RemoveVendorCommandShouldRemoveVendorRaiseVendorRemovedEvent()
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

        [Test]
        public void EditVendorCommandShouldNavigateToVendorDetailViewWithVendorToEditAndDisallowAddEditRemove()
        {
            //arrange
            var vendorListItem = new VendorListItem(new Guid("511e2f63-1878-44db-866d-ba38e6f08d56"), "foo");
            _regionManager.Expect(rm => rm.RequestNavigate(RegionNames.EditVendorRegion, new Uri("VendorDetailView?vendorId=511e2f63-1878-44db-866d-ba38e6f08d56", UriKind.Relative)));

            //act
            _viewModel.EditVendorCommand.Execute(vendorListItem);

            //assert
            _regionManager.VerifyAllExpectations();

            Assert.That(_viewModel.AddVendorCommand.CanExecute(), Is.False);
            Assert.That(_viewModel.EditVendorCommand.CanExecute(null), Is.False);
            Assert.That(_viewModel.RemoveVendorCommand.CanExecute(null), Is.False);
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
        public void VendorUpdatedEventShouldReloadVendorDataAndAllowAddEditRemove()
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

            Assert.That(_viewModel.AddVendorCommand.CanExecute(), Is.True);
            Assert.That(_viewModel.EditVendorCommand.CanExecute(null), Is.True);
            Assert.That(_viewModel.RemoveVendorCommand.CanExecute(null), Is.True);
        }

        [Test]
        public void VendorRemovedEventShouldReloadVendorDataAndAllowAddEditRemove()
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

            Assert.That(_viewModel.AddVendorCommand.CanExecute(), Is.True);
            Assert.That(_viewModel.EditVendorCommand.CanExecute(null), Is.True);
            Assert.That(_viewModel.RemoveVendorCommand.CanExecute(null), Is.True);
        }

        private void AssertDataLoaded()
        {
            var vendors = _viewModel.VendorList;

            Assert.That(vendors.Count, Is.EqualTo(1));
            Assert.That(vendors[0], Is.EqualTo(_updatedViewModelItem));
        }
    }
}