using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HealthAPI.Models
{
    public class RestingHeartRate
    {
        [Key]
        [Column(TypeName = "DateTime")]
        public DateTime DateTime { get; set; }
        public int Beats { get; set; }
        public decimal? MovingAverageBeats { get; set; }
    }
}
