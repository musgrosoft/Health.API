using System;
using System.Collections.Generic;
using System.Linq;
using Repositories.Models;

namespace Services.Health
{
    public class AggregateStatisticsCalculator : IAggregateStatisticsCalculator
    {
        public IList<RestingHeartRate> GetMovingAverages(IList<RestingHeartRate> orderedRestingHeartRates, int period)
        {
            return GetMovingAverages(
                orderedRestingHeartRates,
                period,
                getValue: x => x.Beats,
                setValue: (x, val) => x.MovingAverageBeats = val
            );
        }

        public IList<Weight> GetMovingAverages(IList<Weight> orderedWeights, int period)
        {
            return GetMovingAverages(
                orderedWeights,
                period,
                getValue: x => x.Kg,
                setValue: (x, val) => x.MovingAverageKg = val
            );
        }

        public IList<BloodPressure> GetMovingAverages(IList<BloodPressure> orderedBloodPressures, int period)
        {
            //_logger.Log("BLOOD PRESSURE : Add moving averages (using generic method)");
            var bloodPressuresWithSystolicMovingAverage = 
            GetMovingAverages(
                orderedBloodPressures,
                period,
                getValue: x => x.Systolic,
                setValue: (x, val) => x.MovingAverageSystolic = val
            ).ToList();

            return
            GetMovingAverages(
                bloodPressuresWithSystolicMovingAverage,
                period,
                getValue: x => x.Diastolic,
                setValue: (x, val) => x.MovingAverageDiastolic = val
            );

        }
        
        private IList<T> GetMovingAverages<T>( IList<T> orderedList, int period, Func<T, Double?> getValue, Action<T, Double?> setValue)
        {
            for (int i = 0; i < orderedList.Count(); i++)
            {
                if (i < period - 1)
                {
                    setValue(orderedList[i], null);
                }
                else
                {
                    var average = orderedList.Skip(i - (period - 1)).Take(period).Average(x => getValue(x));
                    setValue(orderedList[i], average);
                }

            }

            return orderedList;

        }



        public IList<StepCount> GetCumSums(IList<StepCount> orderedStepCounts)
        {
            return GetCumSums(
                orderedStepCounts,
                sc => sc.Count,
                sc => sc.CumSumCount,
                (sc, val) => sc.CumSumCount = val
            ).ToList();
        }

        public IList<ActivitySummary> GetCumSums(IList<ActivitySummary> orderedActivitySummaries)
        {
            return GetCumSums(
                orderedActivitySummaries,
                act => act.ActiveMinutes,
                act => act.CumSumActiveMinutes,
                (act, val) => act.CumSumActiveMinutes = val
            ).ToList();
        }

        public IList<HeartRateSummary> GetCumSums(IList<HeartRateSummary> orderedHeartRateSummaries)
        {
            return GetCumSums(
                orderedHeartRateSummaries,
                hs => hs.CardioAndAbove,
                hs => hs.CumSumCardioAndAbove,
                (hs, val) => hs.CumSumCardioAndAbove = val
            ).ToList();
        }
        
        public IList<AlcoholIntake> GetCumSums(IList<AlcoholIntake> orderedAlcoholIntakes)
        {
            //  _logger.Log("UNITS : Calculate cum sum");
            return GetCumSums(
                orderedAlcoholIntakes,
                ai => ai.Units,
                ai => ai.CumSumUnits,
                (ai, val) => ai.CumSumUnits = val
            ).ToList();
        }

        private IList<T> GetCumSums<T>(
            IList<T> orderedList,
            Func<T, Double?> GetValue,
            Func<T, Double?> GetCumSum,
            Action<T, Double?> SetCumSum
        ) where T : class
        {
            for (int i = 0; i < orderedList.Count(); i++)
            {
                Double? value = GetValue(orderedList[i]);

                if (i > 0)
                {
                    value += GetCumSum(orderedList[i - 1]);
                }

                SetCumSum(orderedList[i], value);
            }

            return orderedList;
        }

    }
}