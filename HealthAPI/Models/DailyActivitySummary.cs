using System;
using System.Collections.Generic;

namespace HealthAPI.Models
{
    public class DailyActivitySummary
    {
        public DateTime DateTime { get; set; }
        public int SedentaryMinutes { get; set; }
        public int LightlyActiveMinutes { get; set; }
        public int FairlyActiveMinutes { get; set; }
        public int VeryActiveMinutes { get; set; }
    }
}
