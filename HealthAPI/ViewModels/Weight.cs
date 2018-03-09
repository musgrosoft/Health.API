using System;

namespace HealthAPI.ViewModels
{
    public class Weight
    {
        public DateTime DateTime { get; set; }
        public decimal Kg { get; set; }
        public decimal? MovingAverageKg { get; set; }

    }
}
