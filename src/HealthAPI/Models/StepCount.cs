using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAPI.Models
{
    public class StepCount
    {
        [Key]
        [Column(TypeName = "DateTime")]
        public DateTime DateTime { get; set; }
        public int? Count { get; set; }

        [NotMapped]
        public DateTime Week { get { return DateTime.AddDays(-(int)DateTime.DayOfWeek); } }

    }
}
