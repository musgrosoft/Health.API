using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAPI.Acceptance.Tests.Domain
{
    public class RestingHeartRate
    {
        public DateTime DateTime { get; set; }
        public int Beats { get; set; }
        public decimal? MovingAverageBeats { get; set; }
    }
}
