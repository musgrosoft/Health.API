using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class RestingHeartRate
    {
        [Key]
        [Column(TypeName = "DateTime")]
        public DateTime CreatedDate { get; set; }
        public int Beats { get; set; }
        public decimal? MovingAverageBeats { get; set; }
    }
}
