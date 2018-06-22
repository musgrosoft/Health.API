using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAPI.Acceptance.Tests.Domain
{
    public class AlcoholIntake
    {
        public DateTime DateTime { get; set; }
        public Decimal Units { get; set; }
        public Decimal? CumSumUnits { get; set; }
    }
}
