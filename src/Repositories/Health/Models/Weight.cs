using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Health.Models
{
    public class Weight
    {
        [Key]
        public DateTime CreatedDate { get; set; }
        public Double? Kg { get; set; }
        public Double? FatRatioPercentage { get; set; }

    }

}
