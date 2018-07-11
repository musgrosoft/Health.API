using System;
using System.Collections.Generic;
using System.Linq;
using Repositories.Models;

namespace Services.MyHealth
{
    public class AggregationCalculator : IAggregationCalculator
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



        public IEnumerable<StepCount> GetCumSums(IList<StepCount> orderedStepCounts)
        {
            var localStepCounts = orderedStepCounts.ToList();

            for (int i = 0; i < localStepCounts.Count; i++)
            {
                int? previousCumSum;

                if (i == 0)
                {
                    previousCumSum = 0;
                }
                else
                {
                    previousCumSum = localStepCounts[i - 1].CumSumCount;
                }

                localStepCounts[i].CumSumCount = (localStepCounts[i].Count ?? 0) + previousCumSum;
            }

            return localStepCounts;
        }

        public IEnumerable<ActivitySummary> GetCumSums(IList<ActivitySummary> orderedActivitySummaries)
        {
            var localActivitySummaries = orderedActivitySummaries.ToList();

            for (int i = 0; i < localActivitySummaries.Count; i++)
            {
                int? previousCumSum;

                if (i == 0)
                {
                    previousCumSum = 0;
                }
                else
                {
                    previousCumSum = localActivitySummaries[i - 1].CumSumActiveMinutes;
                }

                localActivitySummaries[i].CumSumActiveMinutes = (localActivitySummaries[i].ActiveMinutes) + previousCumSum;
            }

            return localActivitySummaries;
        }

        public IEnumerable<HeartRateSummary> GetCumSums(IList<HeartRateSummary> orderedHeartSummaries)
        {
            var localHeartSummaries = orderedHeartSummaries.ToList();

            for (int i = 0; i < localHeartSummaries.Count; i++)
            {
                int? previousCumSum;

                if (i == 0)
                {
                    previousCumSum = 0;
                }
                else
                {
                    previousCumSum = localHeartSummaries[i - 1].CumSumCardioAndAbove;
                }

                localHeartSummaries[i].CumSumCardioAndAbove = (localHeartSummaries[i].CardioMinutes) + (localHeartSummaries[i].PeakMinutes) + previousCumSum;
            }

            return localHeartSummaries;
        }
        
        public IEnumerable<AlcoholIntake> GetCumSums(IList<AlcoholIntake> orderedAlcoholIntakes)
        {
            //  _logger.Log("UNITS : Calculate cum sum");
            return GetCumSums(
                orderedAlcoholIntakes,
                ai => ai.Units,
                ai => ai.CumSumUnits,
                (ai, val) => ai.CumSumUnits = val
            );
        }

        private IEnumerable<T> GetCumSums<T>(
            IList<T> orderedList,
            Func<T, Double?> GetValue,
            Func<T, Double?> GetCumSum,
            Action<T, Double?> SetCumSum
        ) where T : class
        {
            //there is no point in this local list, each item inside is still modified (they are not copied)
            var localList = orderedList.ToList();

            for (int i = 0; i < localList.Count(); i++)
            {
                Double? value = GetValue(localList[i]);

                if (i > 0)
                {
                    value += GetCumSum(localList[i - 1]);
                }

                SetCumSum(localList[i], value);
            }

            return localList;

        }

    }
}