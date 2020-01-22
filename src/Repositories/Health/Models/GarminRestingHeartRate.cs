using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Health.Models
{
    public class GarminRestingHeartRate
    {
        [Key]
        public DateTime CreatedDate { get; set; }
        public int Beats { get; set; }

    }
}
