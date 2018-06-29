using System;

namespace HealthAPI.Controllers.Migration
{
    public class TargetWeight
    {
        public DateTime DateTime { get; set; }
        public decimal TargetKg { get; set; }
        public decimal? ActualKg { get; set; }
        public decimal? ActualMovingAverageKg { get; set; }
    }
}