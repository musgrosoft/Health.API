using System;

namespace HealthAPI.Models
{
    public class Weight
    {
        public DateTime DateTime { get; set; }
        public decimal WeightKg { get; set; }
        public decimal? FatRatioPercentage { get; set; }
    }
}
