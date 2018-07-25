using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class Run
    {
        [Key]
        public DateTime CreatedDate { get; set; }
        public TimeSpan Time { get; set; }
        public Double TimeInMinutes => Time.TotalMinutes;
        public Double Metres { get; set; }
        [NotMapped]
        public Double PaceKmPerHour => (60 / Time.TotalMinutes) * (Metres / 1000);
        [NotMapped]
        public TimeSpan PaceTimeFor1Km =>  (Time * 1000) / Metres;
    }
}
