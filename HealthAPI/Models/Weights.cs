using System;
using System.Collections.Generic;

namespace HealthAPI.Models
{
    public class Weight
    {
        public DateTime DateTime { get; set; }
        //public string DataSource { get; set; }
        //public decimal? WeightKg { get; set; }
        public decimal WeightKg { get; set; }
        public decimal? FatRatioPercentage { get; set; }
    }
}
