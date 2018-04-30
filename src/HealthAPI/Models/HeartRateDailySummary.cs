using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAPI.Models
{
    public class HeartRateDailySummary
    {
        public DateTime DateTime { get; set; }
        public int? RestingHeartRate { get; set; }
        public int? OutOfRangeMinutes { get; set; }
        public int? FatBurnMinutes { get; set; }
        public int? CardioMinutes { get; set; }
        public int? PeakMinutes { get; set; }
        
        [NotMapped]
        public DateTime Week { get { return DateTime.AddDays(-(int)DateTime.DayOfWeek); } }
    }
}
