using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class StepCount
    {
        [Key]
        [Column(TypeName = "DateTime")]
        public DateTime CreatedDate { get; set; }
        public int? Count { get; set; }
        public int? CumSumCount { get; set; }
    }
}
