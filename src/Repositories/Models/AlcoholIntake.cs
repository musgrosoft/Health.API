using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class AlcoholIntake
    {
        [Key]
        [Column(TypeName = "DateTime")]
        public DateTime DateTime { get; set; }
        public Decimal Units { get; set; }
        
        public Decimal? CumSumUnits { get; set; }
    }
}
