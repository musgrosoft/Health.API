using System.Collections.Generic;

namespace Services.Fitbit.Domain
{
    public class Value
    {
        public List<object> customHeartRateZones { get; set; }
        public List<HeartRateZone> heartRateZones { get; set; }
        public int restingHeartRate { get; set; }
    }
}