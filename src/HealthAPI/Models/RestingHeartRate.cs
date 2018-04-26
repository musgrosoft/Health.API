using System;
using System.Collections.Generic;

namespace HealthAPI.Models
{
    public class RestingHeartRate
    {
        public DateTime DateTime { get; set; }
        public int Beats { get; set; }
        public decimal? MovingAverageBeats { get; set; }
    }
}
