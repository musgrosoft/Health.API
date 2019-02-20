using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Health.Models
{
    public class Exercise
    {
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public int TotalSeconds { get; set; }
        public int Metres { get; set; }

    }
}
