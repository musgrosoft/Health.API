using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Health.Models
{
    public class Drink
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Double Units { get; set; }
        public Double? Millilitres { get; set; }
        public Double? PercentageAlcohol { get; set; }
        public string Name { get; set; }

    }
}
