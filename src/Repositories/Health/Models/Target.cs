using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Health.Models
{
    public class Target
    {
        [Key]
        public DateTime Date { get; set; }
        public Double Kg  { get; set; }
        public int MetresErgo15Minutes { get; set; }
        public int MetresTreadmill30Minutes { get; set; }
        public int Diastolic { get; set; }
        public int Systolic { get; set; }
        public int Units { get; set; }
        public int CardioMinutes { get; set; }
    }
}

