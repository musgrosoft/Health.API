using System;

namespace Services.Domain
{
    public class TargetAlcoholIntake
    {
        public DateTime DateTime { get; set; }
        public Double TargetCumSumUnits { get; set; }
        public Double? ActualCumSumUnits { get; set; }
    }
}