using System;

namespace HealthAPI.Models
{
    public class HeartRateDailySummary
    {
        public DateTime DateTime { get; set; }
        public int? RestingHeartRate { get; set; }
        public int? OutOfRangeMinutes { get; set; }
        public int? FatBurnMinutes { get; set; }
        public int? CardioMinutes { get; set; }
        public int? PeakMinutes { get; set; }
    }
}
