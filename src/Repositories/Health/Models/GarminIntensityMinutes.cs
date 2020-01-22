using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Health.Models
{
    public class GarminIntensityMinutes
    {
        [Key]
        public DateTime CreatedDate { get; set; }
        public int Minutes { get; set; }

    }
}
