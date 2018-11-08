using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.OAuth.Models
{
    public class Token
    {
        [Key]
        public String Name { get; set; }
        public String Value { get; set; }
 
    }
}