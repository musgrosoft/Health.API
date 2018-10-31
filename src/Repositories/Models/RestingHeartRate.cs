using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class RestingHeartRate
    {
        [Key]
        public DateTime CreatedDate { get; set; }
        public int Beats { get; set; }

    }
}
