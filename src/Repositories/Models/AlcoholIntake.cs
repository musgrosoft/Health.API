using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class AlcoholIntake
    {
        [Key]
        [Column(TypeName = "DateTime")]
        public DateTime CreatedDate { get; set; }
        public Double Units { get; set; }
        
        [NotMapped]
        public Double? CumSumUnits { get; set; }
        [NotMapped]
        public double? TargetCumSumUnits { get; set; }
    }
}
