using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class Weight
    {
        [Key]
        public DateTime CreatedDate { get; set; }
        public Double? Kg { get; set; }
        public Double? FatRatioPercentage { get; set; }

        [NotMapped]
        public Double? MovingAverageKg { get; set; }

        [NotMapped]
        public Double? TargetKg { get; set; }
    }

}
