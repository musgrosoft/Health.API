using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class ActivitySummary
    {
        [Key]
        [Column(TypeName = "DateTime")]
        public DateTime DateTime { get; set; }
        public int SedentaryMinutes { get; set; }
        public int LightlyActiveMinutes { get; set; }
        public int FairlyActiveMinutes { get; set; }
        public int VeryActiveMinutes { get; set; }


        public int CumSumActiveMinutes { get; set; }

        [NotMapped]
        public int ActiveMinutes { get { return FairlyActiveMinutes + VeryActiveMinutes; } }

        [NotMapped]
        public double? Target
        {
            get
            {
                var days = DateTime - new DateTime(2017, 5, 3);

                return days.TotalDays * 30;
            }
        }

        //[NotMapped]
        //public DateTime Week { get { return DateTime.AddDays(-(int)DateTime.DayOfWeek); } }
        //[NotMapped]
        //public DateTime Month { get { return new DateTime(DateTime.Year, DateTime.Month, 1); } }

    }
}
