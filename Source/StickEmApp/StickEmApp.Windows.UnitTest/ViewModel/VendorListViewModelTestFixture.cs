using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using StickEmApp.Dal;
using StickEmApp.Entities;
using StickEmApp.Windows.View;
using StickEmApp.Windows.ViewModel;

namespace StickEmApp.Windows.UnitTest.ViewModel
{
    [TestFixture]
    public class VendorListViewModelTestFixture : UnitOfWorkAwareTestFixture
    {
        private IViewModelFactory _viewModelFactory;
        private IViewManager _viewManager;
        private IVendorRepository _vendorRepository;

        private VendorListViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _viewModelFactory = MockRepository.GenerateMock<IViewModelFactory>();
            _viewManager = MockRepository.GenerateMock<IViewManager>();
            _vendorRepository = MockRepository.GenerateMock<IVendorRepository>();
        }

        [Test]
        public void VendorListViewModelShouldLoadVendorsInVendorList()
        {
            //arrange
            var vendor1 = new Vendor();
            var vendor2 = new Vendor();
            _vendorRepository.Expect(p => p.SelectVendors()).Return(new List<Vendor> {vendor1, vendor2});

            var viewModel1 = new VendorListItemViewModel("test1");
            var viewModel2 = new VendorListItemViewModel("test2");
            _viewModelFactory.Expect(p => p.VendorListItemViewModel(vendor1)).Return(viewModel1);
            _viewModelFactory.Expect(p => p.VendorListItemViewModel(vendor2)).Return(viewModel2);

            //act
            _viewModel = new VendorListViewModel(_viewModelFactory, _viewManager, _vendorRepository);

            //assert
            var vendors = _viewModel.VendorList;

            Assert.That(vendors.Count, Is.EqualTo(2));
            Assert.That(vendors[0], Is.EqualTo(viewModel1));
            Assert.That(vendors[1], Is.EqualTo(viewModel2));
        }

        [Test]
        public void AddVendorCommandShouldShowVendorViewAndRefreshOnClose()
        {
            //arrange
            var openedView = new TestableIView();

            _viewManager.Expect(vf => vf.VendorView()).Return(openedView);
            
            _vendorRepository.Expect(p => p.SelectVendors()).Return(new List<Vendor>()).Repeat.Times(2);
            
            _viewModel = new VendorListViewModel(_viewModelFactory, _viewManager, _vendorRepository);

            //act
            _viewModel.AddVendorCommand.Execute(null);
            openedView.DoClose();

            //assert
            Assert.That(openedView.WasDisplayed, Is.True);
            _viewManager.VerifyAllExpectations();
            _vendorRepository.VerifyAllExpectations();
        }
    }
}