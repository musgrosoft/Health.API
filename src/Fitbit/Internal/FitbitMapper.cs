using System.Collections.Generic;
using System.Linq;
using Fitbit.Internal.Domain;
using Repositories.Health.Models;

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

        internal IEnumerable<SleepState> MapFitbitSleepsToSleepStates(IEnumerable<Domain.FSleep> sleeps)
        {
            var sleepStates = new List<SleepState>();

            foreach (var sleep in sleeps)
            {
                foreach (var dat in sleep.levels.data)
                {
                    var states = new List<SleepState>();

                    for (int i = 0; i < dat.seconds ; i += 30)
                    {
                        var state = new SleepState
                        {
                            CreatedDate = dat.dateTime.AddSeconds(i),
                            State = dat.level
                        };

                        states.Add(state);
                    }

                    sleepStates.AddRange(states);
                }

            }

            return sleepStates;

        }

        internal IEnumerable<SleepSummary> MapFitbitSleepsToSleepSummaries(IEnumerable<Domain.FSleep> sleeps)
        {
            return sleeps.Select(x => new SleepSummary
            {
//                AwakeCount = x.levels.summary.wake?.count,
//                AwakeMinutes = x.levels.summary.wake?.minutes,
//
//                AsleepCount = x.levels.summary.sleep?.count,
//                AsleepMinutes = x.levels.summary.asleep?.minutes,
//
//                RestlessCount = x.levels.summary.restless?.count,
//                RestlessMinutes = x.levels.summary.restless?.minutes,
                

                //DeepCount = x.levels.summary.deep?.count,
                //DeepMinutes = x.levels.summary.deep?.minutes,
                //DeepMinutesThirtyDayAvg = x.levels.summary.deep?.thirtyDayAvgMinutes,

                //LightCount = x.levels.summary.light?.count,
                //LightMinutes = x.levels.summary.light?.minutes,
                //LightMinutesThirtyDayAvg = x.levels.summary.light?.thirtyDayAvgMinutes,

                //RemCount = x.levels.summary.rem?.count,
                //RemMinutes = x.levels.summary.rem?.minutes,
                //RemMinutesThirtyDayAvg = x.levels.summary.rem?.thirtyDayAvgMinutes,

                //WakeCount = x.levels.summary.wake?.count,
                //WakeMinutes = x.levels.summary.wake?.minutes,
                //WakeMinutesThirtyDayAvg = x.levels.summary.wake?.thirtyDayAvgMinutes,


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


    }
}