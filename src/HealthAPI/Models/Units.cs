using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAPI.Models
{
    public partial class Units
    {
        public DateTime DateTime { get; set; }
        public int? Units1 { get; set; }
        [NotMapped]
        public DateTime Week { get { return DateTime.AddDays(-(int)DateTime.DayOfWeek); } }
    }
}
