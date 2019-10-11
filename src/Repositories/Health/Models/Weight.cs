using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Health.Models
{
    public class Weight
    {
        [Key]
        public DateTime CreatedDate { get; set; }
        public Double? Kg { get; set; }
        public Double? FatRatioPercentage { get; set; }

        [NotMapped]
        public Double? FatKg => Kg * FatRatioPercentage / 100;
        [NotMapped]
        public Double? LeanKg => Kg * (100 - FatRatioPercentage) / 100;

    }

}
