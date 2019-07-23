using System;
using System.Collections.Generic;

namespace Importer.Fitbit.Domain
{
    public class FitbitSleeps
    {
        public List<Sleep> sleep { get; set; }
    }

//    public class MinuteData
//    {
//        public string dateTime { get; set; }
//        public string value { get; set; }
//    }

    public class Sleep
    {
        public int awakeCount { get; set; }
        public int awakeDuration { get; set; }
        public int awakeningsCount { get; set; }
        public DateTime dateOfSleep { get; set; }
        public int duration { get; set; }
        public int efficiency { get; set; }
        public DateTime endTime { get; set; }
        public int logId { get; set; }
        //public List<MinuteData> minuteData { get; set; }
        public int minutesAfterWakeup { get; set; }
        public int minutesAsleep { get; set; }
        public int minutesAwake { get; set; }
        public int minutesToFallAsleep { get; set; }
        public int restlessCount { get; set; }
        public int restlessDuration { get; set; }
        public DateTime startTime { get; set; }
        public int timeInBed { get; set; }
    }

}