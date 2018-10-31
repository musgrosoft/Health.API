using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class StepCount
    {
        [Key]
        public DateTime CreatedDate { get; set; }
        public int? Count { get; set; }
        public int? Target { get; set; }

    }
}
