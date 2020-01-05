using System;
using System.Collections.Generic;

namespace Fitbit.Domain
{
    public class FitbitSleepsResponse
    {
        public List<FSleep> sleep { get; set; }
    }

    public class Datum
    {
        public DateTime dateTime { get; set; }
        public string level { get; set; }
        public int seconds { get; set; }
    }

    public class ShortData
    {
        public DateTime dateTime { get; set; }
        public string level { get; set; }
        public int seconds { get; set; }
    }

    public class Deep
    {
        public int count { get; set; }
        public int minutes { get; set; }
        public int thirtyDayAvgMinutes { get; set; }
    }

    public class Light
    {
        public int count { get; set; }
        public int minutes { get; set; }
        public int thirtyDayAvgMinutes { get; set; }
    }

    public class Rem
    {
        public int count { get; set; }
        public int minutes { get; set; }
        public int thirtyDayAvgMinutes { get; set; }
    }

    public class Wake
    {
        public int count { get; set; }
        public int minutes { get; set; }
        public int thirtyDayAvgMinutes { get; set; }
    }

    public class Summary
    {
        public Deep deep { get; set; }
        public Light light { get; set; }
        public Rem rem { get; set; }
        public Wake wake { get; set; }
    }

    public class Levels
    {
        public List<Datum> data { get; set; }
        public List<ShortData> shortData { get; set; }
        public Summary summary { get; set; }
    }

    public class FSleep
    {
        public DateTime dateOfSleep { get; set; }
        public int duration { get; set; }
        public int efficiency { get; set; }
        public DateTime endTime { get; set; }
        public int infoCode { get; set; }
        public bool isMainSleep { get; set; }
        public Levels levels { get; set; }
        public long logId { get; set; }
        public int minutesAfterWakeup { get; set; }
        public int minutesAsleep { get; set; }
        public int minutesAwake { get; set; }
        public int minutesToFallAsleep { get; set; }
        public DateTime startTime { get; set; }
        public int timeInBed { get; set; }
        public string type { get; set; }
    }

}