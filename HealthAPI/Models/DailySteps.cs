using System;

namespace HealthAPI.Models
{
    public class DailySteps
    {
        public DateTime DateTime { get; set; }
        //public string DataSource { get; set; }
        public int? Steps { get; set; }
    }
}
