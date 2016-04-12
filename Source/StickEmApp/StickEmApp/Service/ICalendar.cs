using System;

namespace StickEmApp.Service
{
    public interface ICalendar
    {
        DateTime Now { get; }
    }
}