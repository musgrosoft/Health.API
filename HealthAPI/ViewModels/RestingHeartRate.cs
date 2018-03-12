using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthAPI.ViewModels
{
    public class RestingHeartRate
    {
        public DateTime DateTime { get; set; }
        public int Beats { get; set; }

        public decimal? MovingAverageBeats { get; set; }
    }
}
