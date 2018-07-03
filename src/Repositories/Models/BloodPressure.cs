using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class BloodPressure
    {
        [Key]
        [Column(TypeName = "DateTime")]
        public DateTime CreatedDate { get; set; }
        public int Diastolic { get; set; }
        public int Systolic { get; set; }
        public Decimal? MovingAverageSystolic { get; set; }
        public Decimal? MovingAverageDiastolic { get; set; }
    }
}
