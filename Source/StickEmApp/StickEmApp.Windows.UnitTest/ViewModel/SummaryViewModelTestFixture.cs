using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using StickEmApp.Dal;
using StickEmApp.Entities;
using StickEmApp.Windows.Infrastructure.Events;
using StickEmApp.Windows.ViewModel;

namespace StickEmApp.Windows.UnitTest.ViewModel
{
    [TestFixture]
    public class SummaryViewModelTestFixture : UnitOfWorkAwareTestFixture
    {
        private IStickerSalesPeriodRepository _stickerSalesPeriodRepository;
        private IVendorRepository _vendorRepository;
        private IEventBus _eventBus;

        private StickerSalesPeriod _period;
        private SalesPeriodStatus _status;

        private SummaryViewModel _viewModel;

        private static Action<Guid> _vendorChangedCallback;
        private static Action<Guid> _stickerSalesPeriodChangedCallback;

        [SetUp]
        public void SetUp()
        {
            var vendorList = new List<Vendor>();
            _vendorRepository = MockRepository.GenerateMock<IVendorRepository>();
            _vendorRepository.Expect(p => p.SelectVendors(true)).Return(vendorList);

            _status = new SalesPeriodStatus
            {
                NumberOfStickersToSell = 500,
                NumberOfStickersSold = 30,
                SalesTotal = new Money(150)
            };
            
            _eventBus = MockRepository.GenerateMock<IEventBus>();

            _period = MockRepository.GenerateMock<StickerSalesPeriod>();
            _period.Expect(p => p.CalculateStatus(vendorList)).Return(_status);

            _stickerSalesPeriodRepository = MockRepository.GenerateMock<IStickerSalesPeriodRepository>();
            _stickerSalesPeriodRepository.Expect(repo => repo.Get()).Return(_period).Repeat.Once();

            _vendorChangedCallback = null;
            _eventBus.Expect(p => p.On<VendorChangedEvent, Guid>(Arg<Action<Guid>>.Is.Anything)).WhenCalled(cb => _vendorChangedCallback = (Action<Guid>)cb.Arguments[0]);

            _stickerSalesPeriodChangedCallback = null;
            _eventBus.Expect(p => p.On<StickerSalesPeriodChangedEvent, Guid>(Arg<Action<Guid>>.Is.Anything)).WhenCalled(cb => _stickerSalesPeriodChangedCallback = (Action<Guid>)cb.Arguments[0]);

            _viewModel = new SummaryViewModel(_stickerSalesPeriodRepository, _vendorRepository, _eventBus);
        }

        [Test]
        public void LoadShouldSetViewModelPropertiesFromSalesPeriodResult()
        {
            Assert.That(_viewModel.NumberOfStickersToSell, Is.EqualTo(500));
            Assert.That(_viewModel.NumberOfStickersSold, Is.EqualTo(30));
            Assert.That(_viewModel.SalesTotal, Is.EqualTo(150));
        }

        [Test]
        public void VendorChangeEventShouldUpdateViewModelProperties()
        {
            var updatedVendorList = new List<Vendor>();
            _vendorRepository.Expect(p => p.SelectVendors(false)).Return(updatedVendorList);

            var updatedStatus = new SalesPeriodStatus
            {
                NumberOfStickersToSell = 42,
                NumberOfStickersSold = 37,
                SalesTotal = new Money(35)
            };

            var updatedPeriod = MockRepository.GenerateMock<StickerSalesPeriod>();
            updatedPeriod.Expect(p => p.CalculateStatus(updatedVendorList)).Return(updatedStatus);

            _stickerSalesPeriodRepository.Expect(repo => repo.Get()).Return(updatedPeriod);

            //act
            _vendorChangedCallback(Guid.NewGuid());

            Assert.That(_viewModel.NumberOfStickersToSell, Is.EqualTo(42));
            Assert.That(_viewModel.NumberOfStickersSold, Is.EqualTo(37));
            Assert.That(_viewModel.SalesTotal, Is.EqualTo(35));
        }

        [Test]
        public void SalesPeriodChangeEventShouldUpdateViewModelProperties()
        {
            var updatedVendorList = new List<Vendor>();
            _vendorRepository.Expect(p => p.SelectVendors(false)).Return(updatedVendorList);

            var updatedStatus = new SalesPeriodStatus
            {
                NumberOfStickersToSell = 42,
                NumberOfStickersSold = 37,
                SalesTotal = new Money(35)
            };

            var updatedPeriod = MockRepository.GenerateMock<StickerSalesPeriod>();
            updatedPeriod.Expect(p => p.CalculateStatus(updatedVendorList)).Return(updatedStatus);

            _stickerSalesPeriodRepository.Expect(repo => repo.Get()).Return(updatedPeriod);

            //act
            _stickerSalesPeriodChangedCallback(Guid.NewGuid());

            Assert.That(_viewModel.NumberOfStickersToSell, Is.EqualTo(42));
            Assert.That(_viewModel.NumberOfStickersSold, Is.EqualTo(37));
            Assert.That(_viewModel.SalesTotal, Is.EqualTo(35));
        }
    }
}