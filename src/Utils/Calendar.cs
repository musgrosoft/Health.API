using System;

namespace Utils
{
    public class Calendar : ICalendar
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}