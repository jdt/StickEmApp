using System;
using Prism.Events;

namespace StickEmApp.Windows.Infrastructure.Events
{
    public class VendorChangedEvent : PubSubEvent<Guid>
    {
    }
}