using System;

namespace Repositories.Health.Models
{
    public class HeartRate
    {
        public DateTime CreatedDate { get; set; }
        public string Source { get; set; }
        public int Bpm { get; set; }
    }
}