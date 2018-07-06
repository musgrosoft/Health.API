using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Models
{
    public class Run
    {
        [Key]
        public DateTime CreatedDate { get; set; }
        public TimeSpan Time { get; set; }
        public Double Distance { get; set; }
    }
}
