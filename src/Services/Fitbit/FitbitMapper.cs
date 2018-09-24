﻿using System.Collections.Generic;
using System.Linq;
using Repositories.Models;
using Services.Fitbit.Domain;

namespace Services.Fitbit
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

        public IEnumerable<HeartRateSummary> MapActivitiesHeartsToHeartRateSummaries(IEnumerable<ActivitiesHeart> activitiesHearts)
        {
            return activitiesHearts.Select(x => new HeartRateSummary
            {
                CreatedDate = x.dateTime,
                OutOfRangeMinutes = x.value.heartRateZones.First(y => y.name == "Out of Range").minutes,
                FatBurnMinutes = x.value.heartRateZones.First(y => y.name == "Fat Burn").minutes,
                CardioMinutes = x.value.heartRateZones.First(y => y.name == "Cardio").minutes,
                PeakMinutes = x.value.heartRateZones.First(y => y.name == "Peak").minutes
            });
        }

        public IEnumerable<ActivitySummary> MapFitbitDailyActivitiesToActivitySummaries(IEnumerable<FitbitDailyActivity> fitbitDailyActivities)
        {
            return fitbitDailyActivities.Select(x => new ActivitySummary
            {
                CreatedDate = x.DateTime,
                FairlyActiveMinutes = x.summary.fairlyActiveMinutes,
                LightlyActiveMinutes = x.summary.lightlyActiveMinutes,
                SedentaryMinutes = x.summary.sedentaryMinutes,
                VeryActiveMinutes = x.summary.veryActiveMinutes
            });
        }

        public IEnumerable<StepCount> MapFitbitDailyActivitiesToStepCounts(IEnumerable<FitbitDailyActivity> fitbitDailyActivities)
        {
            return fitbitDailyActivities.Select(x => new StepCount
            {
                CreatedDate = x.DateTime,
                Count = x.summary.steps
            });
        }
    }
}