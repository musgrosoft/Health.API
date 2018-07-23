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
        public Double Metres { get; set; }
        [NotMapped]
        public Double PaceKmPerHour => (60 / Time.TotalMinutes) * (Metres / 1000);
        [NotMapped]
        public Double PaceTimeFor1Km =>  (Metres / 1000) / Time.TotalHours;
    }
}
