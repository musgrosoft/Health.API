using System;

namespace HealthAPI.Acceptance.Tests.Domain
{
    public class Weight
    {
        public DateTime DateTime { get; set; }
        public decimal Kg { get; set; }
        public decimal? FatRatioPercentage { get; set; }
        public decimal? MovingAverageKg { get; set; }
    }
}