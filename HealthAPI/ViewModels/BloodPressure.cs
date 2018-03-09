using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthAPI.ViewModels
{
    public class BloodPressure
    {
        public DateTime DateTime { get; set; }
        
        public int Diastolic { get; set; }
        public int Systolic { get; set; }

        public int? MovingAverageSystolic { get; set; }

        public int? MovingAverageDiastolic { get; set; }

    }
}
