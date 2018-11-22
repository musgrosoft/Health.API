using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Health.Models
{
    public class Exercise
    {
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public TimeSpan Time { get; set; }
        public Double Metres { get; set; }

    }
}
