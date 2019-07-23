using System.Collections.Generic;
using System.Linq;
using Importer.Fitbit.Domain;
using Repositories.Health.Models;

namespace Importer.Fitbit
{
    public class FitbitMapper : IFitbitMapper
    {
        public IEnumerable<RestingHeartRate> MapActivitiesHeartsToRestingHeartRates(IEnumerable<ActivitiesHeart> activitiesHearts)
        {
            return activitiesHearts
                .Where(a => a.value.restingHeartRate != 0)
                .Select(x => new RestingHeartRate
                {
                    CreatedDate = x.dateTime,
                    Beats = x.value.restingHeartRate
                });
        }

        public IEnumerable<MyFitbitSleep> MapSleepsToFitbitSleeps(IEnumerable<Sleep> sleeps)
        {
            return sleeps.Select(x => new MyFitbitSleep
            {
                AwakeCount = x.awakeCount,
                AwakeDuration = x.awakeDuration,
                AwakeningsCount = x.awakeningsCount,
                DateOfSleep = x.dateOfSleep,
                Duration = x.duration,
                Efficiency = x.efficiency,
                EndTime = x.endTime,
                LogId = x.logId,
                MinutesAfterWakeup = x.minutesAfterWakeup,
                MinutesAsleep = x.minutesAsleep,
                MinutesAwake = x.minutesAwake,
                MinutesToFallAsleep = x.minutesToFallAsleep,
                RestlessCount = x.restlessCount,
                RestlessDuration = x.restlessDuration,
                StartTime = x.startTime,
                TimeInBed = x.timeInBed,
                Level1 = x.minuteData.Count(y => y.value == 1),
                Level2 = x.minuteData.Count(y => y.value == 2),
                Level3 = x.minuteData.Count(y => y.value == 3)
            });
        }

        //public IEnumerable<Run> MapFitbitDailyActivitiesToRuns(IEnumerable<FitbitDailyActivity> fitbitDailyActivities)
        //{
        //    var allTheRuns = new List<Run>();

        //    foreach (var fitbitDailyActivity in fitbitDailyActivities)
        //    {
        //        //TimeSpan startTime;
        //        //filter by some indicator that its a run

        //        var someRuns = fitbitDailyActivity.activities.Select(y =>
        //            new Run
        //            {
        //                //add start time
        //                //CreatedDate = TimeSpan.TryParse("07:35", out startTime) ? fitbitDailyActivity.DateTime.Add(startTime) : fitbitDailyActivity.DateTime,
        //                CreatedDate = fitbitDailyActivity.DateTime,
        //                Time = TimeSpan.FromMilliseconds(y.duration),
        //                Metres = y.distance * 1000

        //            });

        //        allTheRuns.AddRange(someRuns);
        //    }

        //    return allTheRuns;
        //}

    }
}