using System;

namespace HealthAPI.Acceptance.Tests.Domain
{
    public class BloodPressure
    {
        public DateTime DateTime { get; set; }
        public int Diastolic { get; set; }
        public int Systolic { get; set; }
        public Decimal? MovingAverageSystolic { get; set; }
        public Decimal? MovingAverageDiastolic { get; set; }
    }
}