using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Health.API.Controllers.Grafana
{
    public class GrafanaWeight
    {
        public double? FatRatioPercentage { get; set; }
        public DateTime Day { get; set; }
        public double? Kg { get; set; }
        public double? MovingAverageKg { get; set; }
        public double? MovingAverageFatRatioPercentage { get; set; }

        //public 
    }
}
