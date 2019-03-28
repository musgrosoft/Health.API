using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Health.Models
{
    public class AlcoholIntake
    {
        [Key]
        public DateTime CreatedDate { get; set; }
        public Double Units { get; set; }
    }
}
