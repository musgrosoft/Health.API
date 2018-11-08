﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Health.Models
{
    public class StepCount
    {
        [Key]
        public DateTime CreatedDate { get; set; }
        public int? Count { get; set; }
        public int? Target { get; set; }

    }
}
