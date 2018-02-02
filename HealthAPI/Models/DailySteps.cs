using System;
using System.Collections.Generic;

namespace HealthAPI.Models
{
    public partial class DailySteps
    {
        public DateTime DateTime { get; set; }
        public string DataSource { get; set; }
        public int? Steps { get; set; }
    }
}
