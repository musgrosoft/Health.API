using System;
using System.Collections.Generic;

namespace Importer.Fitbit.Internal.Domain
{
    internal class FitbitSleeps
    {
        internal List<Sleep> sleep { get; set; }
    }

    //    public class Datum
    //    {
    //        public DateTime dateTime { get; set; }
    //        public string level { get; set; }
    //        public int seconds { get; set; }
    //    }
    //
    //    public class ShortData
    //    {
    //        public DateTime dateTime { get; set; }
    //        public string level { get; set; }
    //        public int seconds { get; set; }
    //    }

    internal class Deep
    {
        internal int count { get; set; }
        internal int minutes { get; set; }
        internal int thirtyDayAvgMinutes { get; set; }
    }

    internal class Light
    {
        internal int count { get; set; }
        internal int minutes { get; set; }
        internal int thirtyDayAvgMinutes { get; set; }
    }

    internal class Rem
    {
        internal int count { get; set; }
        internal int minutes { get; set; }
        internal int thirtyDayAvgMinutes { get; set; }
    }

    internal class Wake
    {
        internal int count { get; set; }
        internal int minutes { get; set; }
        internal int thirtyDayAvgMinutes { get; set; }
    }

    internal class Asleep
    {
        internal int count { get; set; }
        internal int minutes { get; set; }
    }

    internal class Awake
    {
        internal int count { get; set; }
        internal int minutes { get; set; }
    }

    internal class Restless
    {
        internal int count { get; set; }
        internal int minutes { get; set; }
    }

    internal class Summary
    {
        internal Deep deep { get; set; }
        internal Light light { get; set; }
        internal Rem rem { get; set; }
        internal Wake wake { get; set; }
        internal Asleep asleep { get; set; }
        internal Awake awake { get; set; }
        internal Restless restless { get; set; }
    }

    internal class Levels
    {
        //        public List<Datum> data { get; set; }
        //        public List<ShortData> shortData { get; set; }
        internal Summary summary { get; set; }
    }

    internal class Sleep
    {
        internal DateTime dateOfSleep { get; set; }
        internal int duration { get; set; }
        internal int efficiency { get; set; }
        internal DateTime endTime { get; set; }
        internal int infoCode { get; set; }
        internal Levels levels { get; set; }
        internal long logId { get; set; }
        internal int minutesAfterWakeup { get; set; }
        internal int minutesAsleep { get; set; }
        internal int minutesAwake { get; set; }
        internal int minutesToFallAsleep { get; set; }
        internal DateTime startTime { get; set; }
        internal int timeInBed { get; set; }
        internal string type { get; set; }
    }



}