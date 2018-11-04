﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class BloodPressure
    {
        [Key]
        public DateTime CreatedDate { get; set; }
        public Double? Diastolic { get; set; }
        public Double? Systolic { get; set; }
        
    }
}