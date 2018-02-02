using System;
using System.Collections.Generic;

namespace HealthAPI.Models
{
    public partial class HeartRateDailySummaries
    {
        public DateTime DateTime { get; set; }
        public string DataSource { get; set; }
        public int? RestingHeartRate { get; set; }
        public int? OutOfRangeMinutes { get; set; }
        public int? FatBurnMinutes { get; set; }
        public int? CardioMinutes { get; set; }
        public int? PeakMinutes { get; set; }
    }
}
