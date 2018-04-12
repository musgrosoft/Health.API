using System;
using System.Collections.Generic;

namespace HealthAPI.Models
{
    public partial class RestingHeartRate
    {
        public DateTime DateTime { get; set; }
        public int Beats { get; set; }
    }
}
