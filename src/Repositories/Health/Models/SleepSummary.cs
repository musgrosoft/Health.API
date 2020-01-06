using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Health.Models
{
    public class SleepSummary
    {

  //      public long LogId { get; set; }

        
        public DateTime DateOfSleep { get; set; }

        [Key]
        public DateTime StartTime{get;set;}
        public DateTime EndTime { get; set; }
        public bool IsMainSleep { get; set; }








//        public int Duration { get; set; }
//        public int Efficiency { get; set; }
        

        //public int MinutesAfterWakeup { get; set; }
        public int MinutesAsleep {get;set;}
        public int MinutesAwake { get; set; }
        //public int  MinutesToFallAsleep { get; set; }
        
        
        //public int TimeInBed { get; set; }



        //start stages
        //public int? DeepCount { get; set; }
        public int? DeepMinutes { get; set; }
        //public int? DeepMinutesThirtyDayAvg { get; set; }

        //public int? LightCount { get; set; }
        public int? LightMinutes { get; set; }
        //public int? LightMinutesThirtyDayAvg { get; set; }

        //public int? RemCount { get; set; }
        public int? RemMinutes { get; set; }
        //public int? RemMinutesThirtyDayAvg { get; set; }

        //public int? WakeCount { get; set; }
        public int? WakeMinutes { get; set; }
        //public int? WakeMinutesThirtyDayAvg { get; set; }
        //end stages




            //remove?

        //start classic
        // public int? RestlessCount { get; set; }
        // public int? RestlessMinutes { get; set; }
        //
        // public int? AwakeCount { get; set; }
        // public int? AwakeMinutes { get; set; }
        //
        // public int? AsleepCount { get; set; }
        // public int? AsleepMinutes { get; set; }
        //end classic


        public string Type { get; set; }
        public int InfoCode { get; set; }
    }
}