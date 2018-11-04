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
    }
}
