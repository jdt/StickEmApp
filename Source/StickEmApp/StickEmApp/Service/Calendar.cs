using System;
using System.ComponentModel.Composition;

namespace StickEmApp.Service
{
    [Export(typeof(ICalendar))]
    public class Calendar : ICalendar
    {
        public DateTime Now
        {
            get { return DateTime.Now; }
        
        }
    }
}