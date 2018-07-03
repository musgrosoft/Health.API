using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class Weight
    {
        [Key]
        [Column(TypeName = "DateTime")]
        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "decimal(10, 5)")]
        public decimal Kg { get; set; }

        [Column(TypeName = "decimal(10, 5)")]
        public decimal? FatRatioPercentage { get; set; }

        [NotMapped]
        public decimal? MovingAverageKg { get; set; }

        [NotMapped]
        public decimal? TargetKg { get; set; }


    }
}
