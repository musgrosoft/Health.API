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


        public int? CumSumActiveMinutes { get; set; }

        [NotMapped]
        public int ActiveMinutes { get { return FairlyActiveMinutes + VeryActiveMinutes; } }


    }
}
