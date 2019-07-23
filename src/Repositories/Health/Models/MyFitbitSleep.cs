using System;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Health.Models
{
    public class MyFitbitSleep
    {
        [Key]
        public int LogId { get; set; }
        public DateTime StartTime{get;set;}

        public int AwakeCount{get;set;}
        public int AwakeDuration { get; set; }
        public int AwakeningsCount{get;set;}
        public DateTime DateOfSleep { get; set; }
        public int Duration { get; set; }
        public int Efficiency { get; set; }
        public DateTime EndTime{get;set;}

        public int MinutesAfterWakeup { get; set; }
        public int MinutesAsleep {get;set;}
        public int MinutesAwake { get; set; }
        public int  MinutesToFallAsleep { get; set; }
        public int RestlessCount { get; set; }
        public int RestlessDuration { get; set; }
        public int TimeInBed { get; set; }

        public int Level1 { get; set; }
        public int Level2 { get; set; }
        public int Level3 { get; set; }
    }
}