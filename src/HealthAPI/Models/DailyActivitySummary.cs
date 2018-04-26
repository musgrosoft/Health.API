using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAPI.Models
{
    public class DailyActivitySummary
    {
        public DateTime DateTime { get; set; }
        public int SedentaryMinutes { get; set; }
        public int LightlyActiveMinutes { get; set; }
        public int FairlyActiveMinutes { get; set; }
        public int VeryActiveMinutes { get; set; }

        [NotMapped]
        public int ActiveMinutes { get { return FairlyActiveMinutes + VeryActiveMinutes; } }

        [NotMapped]
        public DateTime Week { get { return DateTime.AddDays(-(int)DateTime.DayOfWeek); } }

    }
}
