using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class AlcoholIntake
    {
        [Key]
        public DateTime CreatedDate { get; set; }
        public Double Units { get; set; }
        public Double? Target { get; set; }
                
    }
}
