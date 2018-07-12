using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Models
{
    public class HeartRate
    {
        [Key]
        public DateTime CreatedDate { get; set; }
        public Double Bpm { get; set; }
    }
}