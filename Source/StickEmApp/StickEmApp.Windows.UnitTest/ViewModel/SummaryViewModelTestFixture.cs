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
        private StickerSalesPeriod _period;

        private SummaryViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _stickerSalesPeriodRepository = MockRepository.GenerateMock<IStickerSalesPeriodRepository>();

            _period = new StickerSalesPeriod
            {
                NumberOfStickersToSell = 55
            };
            _stickerSalesPeriodRepository.Expect(repo => repo.Get()).Return(_period);

            _viewModel = new SummaryViewModel(_stickerSalesPeriodRepository);
        }

        [Test]
        public void LoadShouldSetNumberOfStickersToSell()
        {
            Assert.That(_viewModel.NumberOfStickersToSell, Is.EqualTo(55));
        }
    }
}