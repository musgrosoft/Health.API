using System;

namespace HealthAPI.Acceptance.Tests.Domain
{
    public class StepCount
    {
        public DateTime DateTime { get; set; }
        public int? Count { get; set; }
        public int? CumSumCount { get; set; }
    }
}