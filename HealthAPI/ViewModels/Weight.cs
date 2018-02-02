using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthAPI.ViewModels
{
    public class Weight
    {
        public DateTime DateTime { get; set; }
        public decimal Kg { get; set; }
        public decimal? MovingAverageKg { get; set; }

    }
}
