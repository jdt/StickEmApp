using System;
using Prism.Events;

namespace StickEmApp.Windows.Infrastructure.Events
{
    public class VendorUpdatedEvent : PubSubEvent<Guid>
    {
    }
}