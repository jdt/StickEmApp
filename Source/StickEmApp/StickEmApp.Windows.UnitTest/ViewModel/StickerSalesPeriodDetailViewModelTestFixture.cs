using System;
using NUnit.Framework;
using Rhino.Mocks;
using StickEmApp.Dal;
using StickEmApp.Entities;
using StickEmApp.Windows.Infrastructure.Events;
using StickEmApp.Windows.ViewModel;

namespace StickEmApp.Windows.UnitTest.ViewModel
{
    [TestFixture]
    public class StickerSalesPeriodDetailViewModelTestFixture : UnitOfWorkAwareTestFixture
    {
        private IStickerSalesPeriodRepository _stickerSalesPeriodRepository;
        private IEventBus _eventBus;

        private StickerSalesPeriod _period;

        private StickerSalesPeriodDetailViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _period = new StickerSalesPeriod {NumberOfStickersToSell = 55};

            _stickerSalesPeriodRepository = MockRepository.GenerateMock<IStickerSalesPeriodRepository>();
            _stickerSalesPeriodRepository.Expect(repo => repo.Get()).Return(_period);

            _eventBus = MockRepository.GenerateMock<IEventBus>();

            _viewModel = new StickerSalesPeriodDetailViewModel(_stickerSalesPeriodRepository, _eventBus);
        }

        [Test]
        public void LoadShouldPutDataInProperties()
        {
            Assert.That(_viewModel.NumberOfStickersToSell, Is.EqualTo(55));
        }

        [Test]
        public void SaveShouldSaveStickerSalesPeriodAndRaiseStickerSalesPeriodChangedEvent()
        {
            //arrange
            var id = Guid.NewGuid();
            StickerSalesPeriod savedPeriod = null;
            _stickerSalesPeriodRepository.Expect(p => p.Save(Arg<StickerSalesPeriod>.Is.Anything)).WhenCalled(m =>
            {
                savedPeriod = (m.Arguments[0] as StickerSalesPeriod);
                savedPeriod.Id = id;
            });

            //act
            _viewModel.NumberOfStickersToSell = 333;
            _viewModel.SaveChangesCommand.Execute();

            //assert
            Assert.That(savedPeriod.NumberOfStickersToSell, Is.EqualTo(333));
            _eventBus.AssertWasCalled(x => x.Publish<StickerSalesPeriodChangedEvent, Guid>(id));
        }
    }
}