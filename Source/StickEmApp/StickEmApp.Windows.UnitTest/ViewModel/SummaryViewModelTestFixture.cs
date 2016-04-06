using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using StickEmApp.Dal;
using StickEmApp.Entities;
using StickEmApp.Windows.ViewModel;

namespace StickEmApp.Windows.UnitTest.ViewModel
{
    [TestFixture]
    public class SummaryViewModelTestFixture : UnitOfWorkAwareTestFixture
    {
        private IStickerSalesPeriodRepository _stickerSalesPeriodRepository;
        private IVendorRepository _vendorRepository;

        private StickerSalesPeriod _period;
        private SalesPeriodStatus _status;

        private SummaryViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            var vendorList = new List<Vendor>();
            _vendorRepository = MockRepository.GenerateMock<IVendorRepository>();
            _vendorRepository.Expect(p => p.SelectVendors()).Return(vendorList);

            _status = new SalesPeriodStatus
            {
                NumberOfStickersToSell = 500,
                NumberOfStickersSold = 30,
                SalesTotal = new Money(150)
            };

            _period = MockRepository.GenerateMock<StickerSalesPeriod>();
            _period.Expect(p => p.CalculateStatus(vendorList)).Return(_status);

            _stickerSalesPeriodRepository = MockRepository.GenerateMock<IStickerSalesPeriodRepository>();
            _stickerSalesPeriodRepository.Expect(repo => repo.Get()).Return(_period);

            _viewModel = new SummaryViewModel(_stickerSalesPeriodRepository, _vendorRepository);
        }

        [Test]
        public void LoadShouldSetViewModelPropertiesFromSalesPeriodResult()
        {
            Assert.That(_viewModel.NumberOfStickersToSell, Is.EqualTo(500));
            Assert.That(_viewModel.NumberOfStickersSold, Is.EqualTo(30));
            Assert.That(_viewModel.SalesTotal, Is.EqualTo(150));
        }
    }
}