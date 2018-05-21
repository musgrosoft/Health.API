using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class Weight
    {
        [Key]
        [Column(TypeName = "DateTime")]
        public DateTime DateTime { get; set; }

        [Column(TypeName = "decimal(10, 5)")]
        public decimal Kg { get; set; }

        [Column(TypeName = "decimal(10, 5)")]
        public decimal? FatRatioPercentage { get; set; }

        [Column(TypeName = "decimal(10, 5)")]
        public decimal? MovingAverageKg { get; set; }

        public double? Target { get {

                //if (DateTime < new DateTime(2018, 5, 21))
                if (DateTime < new DateTime(2018, 5, 1))
                {
                    return null;
                }
                else
                {
                    //var daysDiff = (DateTime - new DateTime(2018, 5, 21)).TotalDays;
                    //return 90.4 - (daysDiff * 0.017);

                    var daysDiff = (DateTime - new DateTime(2018, 5, 1)).TotalDays;
                    return 90.74 - (daysDiff * 0.017);

                }
                



            } } 
        
    }
}
