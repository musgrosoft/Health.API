using System;
using System.Collections.Generic;

namespace HealthAPI.Models
{
    public partial class DailyActivitySummaries
    {
        public DateTime DateTime { get; set; }
        public string DataSource { get; set; }
        public int? SedentaryMinutes { get; set; }
        public int? LightlyActiveMinutes { get; set; }
        public int? FairlyActiveMinutes { get; set; }
        public int? VeryActiveMinutes { get; set; }
    }
}
