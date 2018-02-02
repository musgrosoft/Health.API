using System;
using System.Collections.Generic;

namespace HealthAPI.Models
{
    public partial class BloodPressures
    {
        public DateTime DateTime { get; set; }
        public string DataSource { get; set; }
        public int? Diastolic { get; set; }
        public int? Systolic { get; set; }
    }
}
