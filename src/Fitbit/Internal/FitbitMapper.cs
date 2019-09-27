using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Fitbit.Internal.Domain;
using Repositories.Health.Models;
using Sleep = Repositories.Health.Models.Sleep;

namespace Fitbit.Internal
{
    internal class FitbitMapper //: IFitbitMapper
    {
        internal IEnumerable<RestingHeartRate> MapActivitiesHeartsToRestingHeartRates(IEnumerable<ActivitiesHeart> activitiesHearts)
        {
            return activitiesHearts
                .Where(a => a.value.restingHeartRate != 0)
                .Select(x => new RestingHeartRate
                {
                    CreatedDate = x.dateTime,
                    Beats = x.value.restingHeartRate
                });
        }

        internal IEnumerable<Sleep> MapSleepsToFitbitSleeps(IEnumerable<Domain.Sleep> sleeps)
        {
            return sleeps.Select(x => new Sleep
            {
                AwakeCount = x.levels.summary.awake?.count,
                AwakeMinutes = x.levels.summary.awake?.minutes,

                AsleepCount = x.levels.summary.asleep?.count,
                AsleepMinutes = x.levels.summary.asleep?.minutes,

                RestlessCount = x.levels.summary.restless?.count,
                RestlessMinutes = x.levels.summary.restless?.minutes,
                

                DeepCount = x.levels.summary.deep?.count,
                DeepMinutes = x.levels.summary.deep?.minutes,
                DeepMinutesThirtyDayAvg = x.levels.summary.deep?.thirtyDayAvgMinutes,

                LightCount = x.levels.summary.light?.count,
                LightMinutes = x.levels.summary.light?.minutes,
                LightMinutesThirtyDayAvg = x.levels.summary.light?.thirtyDayAvgMinutes,

                RemCount = x.levels.summary.rem?.count,
                RemMinutes = x.levels.summary.rem?.minutes,
                RemMinutesThirtyDayAvg = x.levels.summary.rem?.thirtyDayAvgMinutes,

                WakeCount = x.levels.summary.wake?.count,
                WakeMinutes = x.levels.summary.wake?.minutes,
                WakeMinutesThirtyDayAvg = x.levels.summary.wake?.thirtyDayAvgMinutes,


                DateOfSleep = x.dateOfSleep,
                Duration = x.duration,
                Efficiency = x.efficiency,
                EndTime = x.endTime,
                LogId = x.logId,
                MinutesAfterWakeup = x.minutesAfterWakeup,
                MinutesAsleep = x.minutesAsleep,
                MinutesAwake = x.minutesAwake,
                MinutesToFallAsleep = x.minutesToFallAsleep,

                StartTime = x.startTime,
                TimeInBed = x.timeInBed,
                Type = x.type,
                InfoCode = x.infoCode,

                //IsMainSleep = (x.isMainSleep == 1)
            });
        }

        public IEnumerable<Drink> MapFitbitFoodsToDrinks(IEnumerable<Food> foods)
        {
            var drinks = foods.Select(x => new Drink()
            {
                Units = double.Parse(new Regex(@"\(([\d\.]*)\sunits\)").Match(x.loggedFood.name).Groups[1].Value),
                CreatedDate = x.logDate
            });

            var drinksAggregatedByDay = drinks
                .GroupBy(x => x.CreatedDate)
                .Select(x => new Drink
                {
                    CreatedDate = x.Key,
                    Units = x.Sum(y => y.Units)
                }).ToList();

            return drinksAggregatedByDay;
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