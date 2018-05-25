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
        
        [NotMapped]
        public double? Target
        {
            get
            {
                var days = DateTime - new DateTime(2016, 1, 1);

                return days.TotalDays * 5;
            }
        }

        public int? CumSumUnits { get; set; }
    }
}
