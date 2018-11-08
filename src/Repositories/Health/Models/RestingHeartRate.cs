using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Health.Models
{
    public class RestingHeartRate
    {
        [Key]
        public DateTime CreatedDate { get; set; }
        public int Beats { get; set; }

    }
}
