using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Health.Models
{
    public class CalendarDate
    {
        [Key]
        public DateTime Date { get; set; }
    }
}
