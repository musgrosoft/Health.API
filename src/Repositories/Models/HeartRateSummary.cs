using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class HeartRateSummary
    {
        [Key]
        public DateTime CreatedDate { get; set; }
        public int? OutOfRangeMinutes { get; set; }
        public int? FatBurnMinutes { get; set; }
        public int? CardioMinutes { get; set; }
        public int? PeakMinutes { get; set; }
        public int? TargetCardioAndAbove { get; set; }

        [NotMapped]
        public double? CardioAndAbove
        {
            get => CardioMinutes + PeakMinutes;
        }

        [NotMapped]
        public double? CumSumCardioAndAbove { get; set; }
        [NotMapped]
        public double? TargetCumSumCardioAndAbove { get; set; }

        
    }
}
