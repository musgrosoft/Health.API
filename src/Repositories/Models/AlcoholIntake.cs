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
        public double? TargetA
        {
            get
            {
                var days = DateTime - new DateTime(2016, 1, 1);

                return days.TotalDays * 6;
            }
        }

        [NotMapped]
        public double? TargetB
        {
            //5086
            //Mon May 21 2018
            get
            {
                if (DateTime < new DateTime(2018, 5, 21))
                {
                    return null;
                }

                var days = DateTime - new DateTime(2018, 5, 20);

                return 5086 + days.TotalDays * 4;
            }
        }


        public int? CumSumUnits { get; set; }
    }
}
