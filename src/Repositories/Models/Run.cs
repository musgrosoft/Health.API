using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Repositories.Models
{
    public class Run
    {
        [Key]
        [Column(TypeName = "DateTime")]
        public DateTime CreatedDate { get; set; }
        public TimeSpan Time { get; set; }
        public Double Distance { get; set; }
    }
}
