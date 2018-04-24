using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAPI.Models
{
    public class Weight
    {
        [Key]
        [Column(TypeName = "DateTime")]
        public DateTime DateTime { get; set; }

        [Column(TypeName = "decimal(10, 5)")]
        public decimal WeightKg { get; set; }

        [Column(TypeName = "decimal(10, 5)")]
        public decimal? FatRatioPercentage { get; set; }

        [Column(TypeName = "decimal(10, 5)")]
        public decimal? MovingAverageKg { get; set; }
        
    }
}
