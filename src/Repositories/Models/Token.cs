using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Models
{
    public class Token
    {
        [Key]
        public String Name { get; set; }
        public String Value { get; set; }
 
    }
}