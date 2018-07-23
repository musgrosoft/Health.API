using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class Ergo

    {
        [Key]
        public DateTime CreatedDate { get; set; }
        public TimeSpan Time { get; set; }
        public Double Metres { get; set; }

        [NotMapped]
        public TimeSpan Split500m => (Time * 500) / (Metres);
    }
}
