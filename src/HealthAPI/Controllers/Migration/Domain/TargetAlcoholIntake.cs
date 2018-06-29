using System;

namespace HealthAPI.Controllers.Migration
{
    public class TargetAlcoholIntake
    {
        public DateTime DateTime { get; set; }
        public decimal TargetCumSumUnits { get; set; }
        public decimal? ActualCumSumUnits { get; set; }
    }
}