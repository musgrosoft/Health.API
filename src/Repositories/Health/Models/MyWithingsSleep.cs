using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Health.Models
{
    public class MyWithingsSleep
    {
        [Key]
        public int Id { get; set; }
        public string TimeZone { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int WakeUpDuration { get; set; }
        public int LightSleepDuration { get; set; }
        public int DeepSleepDuration { get; set; }
        public int WakeUpCount { get; set; }
        public int DurationToSleep { get; set; }
        public int RemSleepDuration { get; set; }
        public int DurationToWakeUp { get; set; }   
        public int HeartRateAvg { get; set; }
        public int HeartRateMin { get; set; }
        public int HeartRateMax { get; set; }
        public int RespirationRateAvg { get; set; }
        public int RespirationRateMin { get; set; }
        public int RespirationRateMax { get; set; }
        public DateTime ModifiedDate { get; set; }



    }
}
