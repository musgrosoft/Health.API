using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class BloodPressure
    {
        [Key]
        [Column(TypeName = "DateTime")]
        public DateTime DateTime { get; set; }
        public int Diastolic { get; set; }
        public int Systolic { get; set; }
        public int? MovingAverageSystolic { get; set; }
        public int? MovingAverageDiastolic { get; set; }
    }
}
