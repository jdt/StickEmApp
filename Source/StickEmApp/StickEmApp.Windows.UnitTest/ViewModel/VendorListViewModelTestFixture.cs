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
        public void VendorUpdateEventShouldReloadVendorData()
        {
            //arrange
            var vendorList = new List<Vendor> { new Vendor() };
            var updatedVendorList = new List<Vendor> { new Vendor() };
            _vendorRepository.Expect(p => p.SelectVendors()).Return(vendorList).Repeat.Once();
            _vendorRepository.Expect(p => p.SelectVendors()).Return(updatedVendorList).Repeat.Once();

            var viewModelItem = new VendorListItem(Guid.NewGuid(), "test1");
            var viewModelList = new List<VendorListItem> { viewModelItem };
            _vendorListItemBuilder.Expect(p => p.BuildFrom(vendorList)).Return(viewModelList);

            var addedViewModelItem = new VendorListItem(Guid.NewGuid(), "test2");
            var updatedViewModelList = new List<VendorListItem> { viewModelItem, addedViewModelItem };
            _vendorListItemBuilder.Expect(p => p.BuildFrom(updatedVendorList)).Return(updatedViewModelList);

            Action<Guid> callback = null;
            _vendorUpdatedEvent.Expect(
                p =>
                    p.Subscribe(Arg<Action<Guid>>.Is.Anything, Arg<ThreadOption>.Is.Anything, Arg<bool>.Is.Anything,
                        Arg<Predicate<Guid>>.Is.Anything))
                .WhenCalled(cb => callback = (Action<Guid>) cb.Arguments[0])
                .Return(null);

            _viewModel = new VendorListViewModel(_vendorRepository, _vendorListItemBuilder, _regionManager, _eventAggregator);

            //act
            callback(Guid.NewGuid());

            //assert
            var vendors = _viewModel.VendorList;

            Assert.That(vendors.Count, Is.EqualTo(2));
            Assert.That(vendors[0], Is.EqualTo(viewModelItem));
            Assert.That(vendors[1], Is.EqualTo(addedViewModelItem));
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

        [Test]
        public void RemoveVendorCommandShouldRemoveVendorAndRaiseVendorRemovedEvent()
        {
            _vendorListItemBuilder.Expect(b => b.BuildFrom(null)).Return(new List<VendorListItem>());

            _viewModel = new VendorListViewModel(_vendorRepository, _vendorListItemBuilder, _regionManager, _eventAggregator);

            var generatedGuid = Guid.NewGuid();
            
            var returnedEvent = MockRepository.GenerateMock<VendorRemovedEvent>();
            returnedEvent.Expect(p => p.Publish(generatedGuid));

            _eventAggregator.Expect(ea => ea.GetEvent<VendorRemovedEvent>()).Return(returnedEvent);

            var removedVendor = new Vendor
            {
                Id = generatedGuid
            };

            var removedItemid = Guid.NewGuid();
            var removedItem = new VendorListItem(removedItemid, "test");
            _vendorRepository.Expect(p => p.Get(removedItemid)).Return(removedVendor);

            //act
            _viewModel.RemoveVendorCommand.Execute(removedItem);

            //assert
            _regionManager.VerifyAllExpectations();
            _eventAggregator.VerifyAllExpectations();
            returnedEvent.VerifyAllExpectations();
        }
    }
}