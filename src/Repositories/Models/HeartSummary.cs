using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class HeartSummary
    {
        [Key]
        [Column(TypeName = "DateTime")]
        public DateTime DateTime { get; set; }
        //public int? RestingHeartRate { get; set; }
        public int? OutOfRangeMinutes { get; set; }
        public int? FatBurnMinutes { get; set; }
        public int? CardioMinutes { get; set; }
        public int? PeakMinutes { get; set; }

        public int? CumSumFatBurnAndAbove { get; set; }
        public int? CumSumCardioAndAbove { get; set; }

        [NotMapped]
        public double? TargetFatBurnAndAbove
        {
            get
            {
                var days = DateTime - new DateTime(2017, 5, 3);

                return days.TotalDays * 100;
            }
        }

        [NotMapped]
        public double? TargetCardioAndAboveA
        {
            get
            {
                var days = DateTime - new DateTime(2017, 5, 3);

                return days.TotalDays * 5;
            }
        }

        [NotMapped]
        public double? TargetCardioAndAboveB
        {
            get
            {
                if (DateTime < new DateTime(2018, 5, 19))
                {
                    return null;
                }

                var days = DateTime - new DateTime(2018, 5, 19);

                return 1775 + days.TotalDays * 8.6;
            }
        }
        //[NotMapped]
        //public DateTime Week { get { return DateTime.AddDays(-(int)DateTime.DayOfWeek); } }
    }
}
