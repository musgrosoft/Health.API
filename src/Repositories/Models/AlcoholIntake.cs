using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class AlcoholIntake
    {
        [Key]
        [Column(TypeName = "DateTime")]
        public DateTime DateTime { get; set; }
        public int Units { get; set; }
        //[NotMapped]
        //public DateTime Week { get { return DateTime.AddDays(-(int)DateTime.DayOfWeek); } }

        [NotMapped]
        public double? Target
        {
            get
            {
                var days = DateTime - new DateTime(2017, 5, 3);

                return days.TotalDays * 4;
            }
        }
    }
}
