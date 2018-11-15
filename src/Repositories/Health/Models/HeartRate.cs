using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Health.Models
{
    public class HeartRate
    {
        public DateTime CreatedDate { get; set; }
        [MaxLength(20)]
        public string Source { get; set; }
        public int Bpm { get; set; }
    }
}