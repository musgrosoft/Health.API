using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAPI.Models
{
    public class AlcoholIntake
    {
        [Key]
        [Column(TypeName = "DateTime")]
        public DateTime DateTime { get; set; }
        public int? Units { get; set; }
    }
}
