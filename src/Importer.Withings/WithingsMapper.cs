using System;
using System.Collections.Generic;
using System.Linq;
using Importer.Withings.Domain;
using Repositories.Health.Models;
using Utils;

namespace Importer.Withings
{
    public class WithingsMapper : IWithingsMapper
    {
        private const int WeightKgMeasureTypeId = 1;
        private const int FatRatioPercentageMeasureTypeId = 6;
        private const int DiastolicBloodPressureMeasureTypeId = 9;
        private const int SystolicBloodPressureMeasureTypeId = 10;

        public IEnumerable<Weight> MapToWeights(IEnumerable<Response.Measuregrp> weightMeasuresGroups)
        {
            return weightMeasuresGroups
                .Where(x=>x.measures.Any(y=>y.type == WeightKgMeasureTypeId))
                .Select(x => new Weight
            {
                CreatedDate = x.date.ToDateFromUnixTime(),
                Kg = x.measures.First(w => w.type == WeightKgMeasureTypeId).value * Math.Pow(10, x.measures.First(w => w.type == WeightKgMeasureTypeId).unit),

                //set if available
                FatRatioPercentage =
                    x.measures.FirstOrDefault(w => w.type == FatRatioPercentageMeasureTypeId) == null 
                    ? null 
                    : (double?)(x.measures.First(w => w.type == FatRatioPercentageMeasureTypeId).value * Math.Pow(10, x.measures.First(w => w.type == FatRatioPercentageMeasureTypeId).unit))
            });
        }

        public IEnumerable<BloodPressure> MapToBloodPressures(IEnumerable<Response.Measuregrp> bloodPressureMeasuresGroups)
        {
            return bloodPressureMeasuresGroups
                .Where(x => x.measures.Any(y => y.type == DiastolicBloodPressureMeasureTypeId))
                .Select(x => new BloodPressure
            {
                CreatedDate = x.date.ToDateFromUnixTime(),
                Diastolic = (int)(x.measures.First(bp => bp.type == DiastolicBloodPressureMeasureTypeId).value * Math.Pow(10, x.measures.First(bp => bp.type == DiastolicBloodPressureMeasureTypeId).unit)),
                Systolic = (int)(x.measures.First(bp => bp.type == SystolicBloodPressureMeasureTypeId).value * Math.Pow(10, x.measures.First(bp => bp.type == SystolicBloodPressureMeasureTypeId).unit)),
            });
        }

        public IEnumerable<MyWithingsSleep> MapToMyWithingsSleep(IEnumerable<Series> series)
        {
            return series.Select(x => new MyWithingsSleep
            {
                DeepSleepDuration = x.data.deepsleepduration,
                DurationToSleep = x.data.durationtosleep,
                DurationToWakeUp = x.data.durationtowakeup,
                EndDate = x.enddate.ToDateFromUnixTime(),
                HeartRateAvg = x.data.hr_average,
                HeartRateMax = x.data.hr_max,
                HeartRateMin = x.data.hr_min,
                Id = x.id,
                LightSleepDuration = x.data.lightsleepduration,
                ModifiedDate = x.modified.ToDateFromUnixTime(),
                RemSleepDuration = x.data.remsleepduration,
                RespirationRateAvg = x.data.rr_average,
                RespirationRateMax = x.data.rr_max,
                RespirationRateMin = x.data.rr_min,
                StartDate = x.startdate.ToDateFromUnixTime(),
                TimeZone = x.timezone,
                WakeUpCount = x.data.wakeupcount,
                WakeUpDuration = x.data.wakeupduration

            });
        }
    }
}