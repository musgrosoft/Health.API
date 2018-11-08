using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Health.Models
{
    public class BloodPressure
    {
        [Key]
        public DateTime CreatedDate { get; set; }
        public Double? Diastolic { get; set; }
        public Double? Systolic { get; set; }
        
    }
}
