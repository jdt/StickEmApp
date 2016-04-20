﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using StickEmApp.Dal;
using StickEmApp.Entities;
using StickEmApp.Service;
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
        private IWindowManager _windowManager;
        private IEventBus _eventBus;
        private IExcelExporter _excelExporter;
        private IResourceManager _resourceManager;

        private VendorListItem _viewModelItem;

        private VendorListViewModel _viewModel;
        private List<Vendor> _vendorList;

        [SetUp]
        public void SetUp()
        {
            _vendorRepository = MockRepository.GenerateMock<IVendorRepository>();
            _vendorListItemBuilder = MockRepository.GenerateMock<IVendorListItemBuilder>();
            _windowManager = MockRepository.GenerateMock<IWindowManager>();
            _eventBus = MockRepository.GenerateMock<IEventBus>();
            _excelExporter = MockRepository.GenerateMock<IExcelExporter>();
            _resourceManager = MockRepository.GenerateMock<IResourceManager>();

            _vendorList = new List<Vendor> { new Vendor() };
            _vendorRepository.Expect(p => p.SelectVendors(false)).Return(_vendorList);

            _viewModelItem = new VendorListItem(Guid.NewGuid(), "test1");
            var viewModelList = new List<VendorListItem> { _viewModelItem };
            _vendorListItemBuilder.Expect(p => p.BuildFrom(_vendorList)).Return(viewModelList);

            _viewModel = new VendorListViewModel(_vendorRepository, _vendorListItemBuilder, _windowManager, _eventBus, _excelExporter, _resourceManager);
        }

        [Test]
        public void FinishedVendorsShouldNotbeShown()
        {
            Assert.That(_viewModel.ShowFinishedVendors, Is.False);
        }

        [Test]
        public void VendorListViewModelShouldLoadVendorsInVendorList()
        {
            //act
            var vendors = _viewModel.VendorList;
            
            //assert
            Assert.That(vendors.Count, Is.EqualTo(1));
        }
        
        [Test]
        public void AddVendorCommandShouldNavigateToVendorDetailViewAndDisallowAddEditRemove()
        {
            _windowManager.Expect(wm => wm.DisplayAddVendor());

            //act
            _viewModel.AddVendorCommand.Execute();

            //assert
            _windowManager.VerifyAllExpectations();
            Assert.That(_viewModel.AddVendorCommand.CanExecute(), Is.False);
            Assert.That(_viewModel.EditVendorCommand.CanExecute(null), Is.False);
            Assert.That(_viewModel.RemoveVendorCommand.CanExecute(null), Is.False);
            Assert.That(_viewModel.EditStickerSalesPeriodCommand.CanExecute(), Is.False);
        }

        [Test]
        public void RemoveVendorCommandShouldRemoveVendorAndRaiseVendorChangedEvent()
        {
            var removedVendorId = Guid.NewGuid();
            var removedVendor = new Vendor
            {
                Id = removedVendorId
            };
            
            _vendorRepository.Expect(p => p.Get(removedVendorId)).Return(removedVendor);
            _eventBus.Expect(ea => ea.Publish<VendorChangedEvent, Guid>(removedVendorId));
            
            var removedItem = new VendorListItem(removedVendorId, "test");
            
            //act
            _viewModel.RemoveVendorCommand.Execute(removedItem);

            //assert
            _windowManager.VerifyAllExpectations();
            _eventBus.VerifyAllExpectations();
        }

        [Test]
        public void EditVendorCommandShouldNavigateToVendorDetailViewWithVendorToEditAndDisallowAddEditRemove()
        {
            //arrange
            var vendorListItem = new VendorListItem(Guid.NewGuid(), "foo");
            _windowManager.Expect(wm => wm.DisplayEditVendor(vendorListItem.Id));

            //act
            _viewModel.EditVendorCommand.Execute(vendorListItem);

            //assert
            _windowManager.VerifyAllExpectations();

            Assert.That(_viewModel.AddVendorCommand.CanExecute(), Is.False);
            Assert.That(_viewModel.EditVendorCommand.CanExecute(null), Is.False);
            Assert.That(_viewModel.RemoveVendorCommand.CanExecute(null), Is.False); 
            Assert.That(_viewModel.EditStickerSalesPeriodCommand.CanExecute(), Is.False);
        }

        [Test]
        public void EditStickerSalesPeriodCommandShouldNavigateToStickerSalesPeriodDetailViewAndDisallowAddEditRemove()
        {
            //arrange
            _windowManager.Expect(wm => wm.DisplayEditStickerSalesPeriod());

            //act
            _viewModel.EditStickerSalesPeriodCommand.Execute();

            //assert
            _windowManager.VerifyAllExpectations();

            Assert.That(_viewModel.AddVendorCommand.CanExecute(), Is.False);
            Assert.That(_viewModel.EditVendorCommand.CanExecute(null), Is.False);
            Assert.That(_viewModel.RemoveVendorCommand.CanExecute(null), Is.False);
            Assert.That(_viewModel.EditStickerSalesPeriodCommand.CanExecute(), Is.False);
        }

        [Test]
        public void ExportToExcelCommandShouldExportToChosenLocationAndDisplayConfirmation()
        {
            //arrange
            const string exportLocation = "EXPORT_LOCATION";

            _windowManager.Expect(wm => wm.DisplayFileSelection()).Return(exportLocation);

            const string confirmationMessage = "CONFIRM_MESSAGE '{0}'";
            _resourceManager.Expect(rm => rm.GetString("DataExportedToExcelFileAt")).Return(confirmationMessage);

            _windowManager.Expect(wm => wm.DisplayConfirmation("CONFIRM_MESSAGE 'EXPORT_LOCATION'"));

            //act
            _viewModel.ExportToExcelCommand.Execute();

            //assert
            _windowManager.VerifyAllExpectations();
            _excelExporter.AssertWasCalled(e => e.Export(_vendorList, exportLocation));
        }
    }

    [TestFixture]
    public class VendorListViewModelEventSubscriptionTestFixture : UnitOfWorkAwareTestFixture
    {
        private IVendorRepository _vendorRepository;
        private IVendorListItemBuilder _vendorListItemBuilder;
        private IWindowManager _windowManager;
        private IEventBus _eventBus;
        private IExcelExporter _excelExporter;
        private IResourceManager _resourceManager;

        private VendorListViewModel _viewModel;
        private VendorListItem _updatedViewModelItem;

        [SetUp]
        public void SetUp()
        {
            _vendorRepository = MockRepository.GenerateMock<IVendorRepository>();
            _vendorListItemBuilder = MockRepository.GenerateMock<IVendorListItemBuilder>();
            _windowManager = MockRepository.GenerateMock<IWindowManager>();
            _eventBus = MockRepository.GenerateMock<IEventBus>();
            _excelExporter = MockRepository.GenerateMock<IExcelExporter>();
            _resourceManager = MockRepository.GenerateMock<IResourceManager>();
            
            var updatedVendorList = new List<Vendor> { new Vendor() };
            _vendorRepository.Expect(p => p.SelectVendors(false)).Return(updatedVendorList);

            _updatedViewModelItem = new VendorListItem(Guid.NewGuid(), "test2");
            var updatedViewModelList = new List<VendorListItem> { _updatedViewModelItem };
            _vendorListItemBuilder.Expect(p => p.BuildFrom(updatedVendorList)).Return(updatedViewModelList);

            _windowManager.Expect(wm => wm.DisplaySummary());

            _viewModel = new VendorListViewModel(_vendorRepository, _vendorListItemBuilder, _windowManager, _eventBus, _excelExporter, _resourceManager);
        }
        
        [Test]
        public void VendorChangedEventShouldReloadVendorDataDisplaySummaryAndAllowAddEditRemoveEditSalesPeriod()
        {
            //arrange
            Action<Guid> callback = null;
            _eventBus.Expect(
                p =>
                    p.On<VendorChangedEvent, Guid>(Arg<Action<Guid>>.Is.Anything))
                .WhenCalled(cb => callback = (Action<Guid>)cb.Arguments[0]);

            _viewModel = new VendorListViewModel(_vendorRepository, _vendorListItemBuilder, _windowManager, _eventBus, _excelExporter, _resourceManager);

            //act
            callback(Guid.NewGuid());

            //assert
            DoAssert();
        }

        [Test]
        public void SalesPeriodChangedEventShouldReloadVendorDataDisplaySummaryAndAllowAddEditRemoveEditSalesPeriod()
        {
            //arrange
            Action<Guid> callback = null;
            _eventBus.Expect(
                p =>
                    p.On<StickerSalesPeriodChangedEvent, Guid>(Arg<Action<Guid>>.Is.Anything))
                .WhenCalled(cb => callback = (Action<Guid>)cb.Arguments[0]);

            _viewModel = new VendorListViewModel(_vendorRepository, _vendorListItemBuilder, _windowManager, _eventBus, _excelExporter, _resourceManager);

            //act
            callback(Guid.NewGuid());

            //assert
            DoAssert();
        }
        
        private void DoAssert()
        {
            var vendors = _viewModel.VendorList;

            Assert.That(vendors.Count, Is.EqualTo(1));
            Assert.That(vendors[0], Is.EqualTo(_updatedViewModelItem));
            
            Assert.That(_viewModel.AddVendorCommand.CanExecute(), Is.True);
            Assert.That(_viewModel.EditVendorCommand.CanExecute(null), Is.True);
            Assert.That(_viewModel.RemoveVendorCommand.CanExecute(null), Is.True); 
            Assert.That(_viewModel.EditStickerSalesPeriodCommand.CanExecute(), Is.True);
        }
    }
}