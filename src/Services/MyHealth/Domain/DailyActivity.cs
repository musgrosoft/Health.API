using System;

namespace Services.MyHealth.Domain
{
    public class DailyActivity  
    {
        public DateTime DateTime { get; set; }
        public int FairlyActiveMinutes { get; set; }
        public int LightlyActiveMinutes { get; set; }
        public int SedentaryMinutes { get; set; }
        public int VeryActiveMinutes { get; set; }
    }
}