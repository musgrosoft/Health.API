﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class StepCount
    {
        [Key]
        [Column(TypeName = "DateTime")]
        public DateTime DateTime { get; set; }
        public int? Count { get; set; }
        public int? CumSumCount { get; set; }

        public double? Target
        {
            get
            {
                var days = DateTime.Now - new DateTime(2017, 5, 4);

                return days.TotalDays * 10000;
            }
        }

        //[NotMapped]
        //public DateTime Week { get { return DateTime.AddDays(-(int)DateTime.DayOfWeek); } }

    }
}
