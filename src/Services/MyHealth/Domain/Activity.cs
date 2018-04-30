using System;

namespace Services.MyHealth.Domain
{
    public class Activity
    {
        public DateTime DateTime { get; set; }
        public int ActiveMinutes { get; set; }
    }
}