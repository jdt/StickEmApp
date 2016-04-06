using System;
using NUnit.Framework;
using Prism.Events;
using Rhino.Mocks;
using StickEmApp.Windows.Infrastructure.Events;

namespace StickEmApp.Windows.UnitTest.Infrastructure.Events
{
    [TestFixture]
    public class EventBusTestFixture
    {
        private IEventAggregator _eventAggregator;

        private EventBus _eventBus;

        [SetUp]
        public void SetUp()
        {
            _eventAggregator = MockRepository.GenerateMock<IEventAggregator>();

            _eventBus = new EventBus(_eventAggregator);
        }

        [Test]
        public void OnShouldSubscribeToEvent()
        {
            //arrange
            var callbackAction = MockRepository.GenerateMock<Action<Guid>>();

            var targetEvent = MockRepository.GenerateMock<VendorChangedEvent>();
            targetEvent.Expect(p => p.Subscribe(callbackAction)).Return(null);

            _eventAggregator.Expect(p => p.GetEvent<VendorChangedEvent>()).Return(targetEvent);

            //act
            _eventBus.On<VendorChangedEvent, Guid>(callbackAction);

            //assert
            targetEvent.VerifyAllExpectations();
        }

        [Test]
        public void PublishShouldRaiseEventWithParameter()
        {
            //arrange
            var parameter = Guid.NewGuid();

            var targetEvent = MockRepository.GenerateMock<VendorChangedEvent>();
            targetEvent.Expect(p => p.Publish(parameter));

            _eventAggregator.Expect(p => p.GetEvent<VendorChangedEvent>()).Return(targetEvent);

            //act
            _eventBus.Publish<VendorChangedEvent, Guid>(parameter);

            //assert
            targetEvent.VerifyAllExpectations();
        }
    }
}