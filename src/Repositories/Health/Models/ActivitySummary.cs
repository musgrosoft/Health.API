﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class ActivitySummary
    {
        [Key]
        public DateTime CreatedDate { get; set; }
        public int SedentaryMinutes { get; set; }
        public int LightlyActiveMinutes { get; set; }
        public int FairlyActiveMinutes { get; set; }
        public int VeryActiveMinutes { get; set; }
        public int? TargetActiveMinutes { get; set; }
        
    }
}