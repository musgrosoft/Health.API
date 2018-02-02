using System;
using System.Collections.Generic;

namespace HealthAPI.Models
{
    public partial class Weights
    {
        public DateTime DateTime { get; set; }
        public string DataSource { get; set; }
        //public decimal? WeightKg { get; set; }
        public decimal WeightKg { get; set; }
        public decimal? FatRatioPercentage { get; set; }
    }
}
