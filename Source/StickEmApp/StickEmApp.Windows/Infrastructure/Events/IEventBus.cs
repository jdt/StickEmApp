using System;
using Prism.Events;

namespace StickEmApp.Windows.Infrastructure.Events
{
    public interface IEventBus
    {
        void On<TPubSubEvent, T>(Action<T> onAction) where TPubSubEvent : PubSubEvent<T>, new();
        void Publish<TPubSubEvent, T>(T parameter) where TPubSubEvent : PubSubEvent<T>, new();
    }
}