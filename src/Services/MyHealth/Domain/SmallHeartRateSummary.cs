using System;

namespace Services.MyHealth.Domain
{
    public class SmallHeartRateSummary
    {
        public DateTime DateTime { get; set; }
        //todo does RestingHeartRate belong in this class
        public int RestingHeartRate { get; set; }
        public int OutOfRangeMinutes { get; set; }
        public int FatBurnMinutes { get; set; }
        public int CardioMinutes { get; set; }
        public int PeakMinutes { get; set; }

    }
}