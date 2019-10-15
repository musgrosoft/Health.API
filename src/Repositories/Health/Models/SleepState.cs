using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Repositories.Health.Models
{
    public class SleepState
    {
        [Key]
        public DateTime CreatedDate { get; set; }
        public string State { get; set; }
    }
}
