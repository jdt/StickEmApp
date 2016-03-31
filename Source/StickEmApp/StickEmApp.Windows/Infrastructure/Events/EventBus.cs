using System;
using System.ComponentModel.Composition;
using Prism.Events;

namespace StickEmApp.Windows.Infrastructure.Events
{
    [Export(typeof(IEventBus))]
    public class EventBus : IEventBus
    {
        private readonly IEventAggregator _eventAggregator;

        [ImportingConstructor]
        public EventBus(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void On<TPubSubEvent, T>(Action<T> onAction) where TPubSubEvent : PubSubEvent<T>, new()
        {
            _eventAggregator.GetEvent<TPubSubEvent>().Subscribe(onAction);
        }

        public void Publish<TPubSubEvent, T>(T parameter) where TPubSubEvent : PubSubEvent<T>, new()
        {
            _eventAggregator.GetEvent<TPubSubEvent>().Publish(parameter);
        }
    }
}