using System;
using System.Collections.Generic;
using System.Linq;
using Repositories.Models;

namespace Services.MyHealth
{
    public class AggregationCalculator : IAggregationCalculator
    {
        
        public IEnumerable<BloodPressure> GetMovingAverages(IList<BloodPressure> seedBloodPressures, IList<BloodPressure> orderedBloodPressures, int period)
        {
            //_logger.Log("BLOOD PRESSURE : Add moving averages (using generic method)");
            var bloodPressuresWithSystolicMovingAverage = 
            GetMovingAverages(
                seedBloodPressures,
                orderedBloodPressures,
                period,
                getValue: x => x.Systolic,
                setValue: (x, val) => x.MovingAverageSystolic = val
            ).ToList();

            return
            GetMovingAverages(
                seedBloodPressures,
                bloodPressuresWithSystolicMovingAverage,
                period,
                getValue: x => x.Diastolic,
                setValue: (x, val) => x.MovingAverageDiastolic = val
            );

        }

        public IEnumerable<RestingHeartRate> GetMovingAverages(IList<RestingHeartRate> seedRestingHeartRates, IList<RestingHeartRate> orderedRestingHeartRates, int period)
        {
            return GetMovingAverages(
                seedRestingHeartRates,
                orderedRestingHeartRates,
                period,
                getValue: x => x.Beats,
                setValue: (x, val) => x.MovingAverageBeats = val
            );
        }

        public IList<Weight> GetMovingAverages(IList<Weight> seedWeights, IList<Weight> orderedWeights, int period)
        {
            return GetMovingAverages(
                seedWeights,
                orderedWeights,
                period,
                getValue: x => x.Kg,
                setValue: (x, val) => x.MovingAverageKg = val
                );
        }

        public IList<Weight> GetMovingAverages(IList<Weight> orderedWeights, int period)
        {
            return GetMovingAverages(orderedWeights.Take(9).ToList(), orderedWeights, period);
        }


        private IList<T> GetMovingAverages<T>(IList<T> seedList, IList<T> orderedList, int period, Func<T, Decimal?> getValue, Action<T, Decimal?> setValue)
        {
            var localOrderedList = orderedList.ToList();

            if (seedList.Count > period - 1)
            {
                seedList = seedList.TakeLast(period - 1).ToList();
            }

            var numberOfMissingKgs = period - 1 - seedList.Count;

            List<T> all = seedList.ToList();
            all.AddRange(localOrderedList);

            for (int i = 0; i < localOrderedList.Count(); i++)
            {
                if (i < numberOfMissingKgs)
                {
                    setValue(localOrderedList[i], null);
                    //                    orderedTs[i].MovingAverageKg = null;
                }
                else
                {
                    var average = all.Skip(i - numberOfMissingKgs).Take(period).Average(x => getValue(x));
                    setValue(localOrderedList[i], average);

                    //orderedTs[i].MovingAverageKg = allWeights.Skip(i - numberOfMissingKgs).Take(period).Average(x => x.Kg);
                }

            }

            return localOrderedList;

        }



        public IEnumerable<StepCount> GetCumSums(StepCount seed, IList<StepCount> orderedStepCounts)
        {
            var localStepCounts = orderedStepCounts.ToList();

            for (int i = 0; i < localStepCounts.Count; i++)
            {
                int? previousCumSum;

                if (i == 0)
                {
                    previousCumSum = (seed?.CumSumCount ?? 0);
                }
                else
                {
                    previousCumSum = localStepCounts[i - 1].CumSumCount;
                }

                localStepCounts[i].CumSumCount = (localStepCounts[i].Count ?? 0) + previousCumSum;
            }

            return localStepCounts;
        }

        public IEnumerable<ActivitySummary> GetCumSums(ActivitySummary seed, IList<ActivitySummary> orderedActivitySummaries)
        {
            var localActivitySummaries = orderedActivitySummaries.ToList();

            for (int i = 0; i < localActivitySummaries.Count; i++)
            {
                int? previousCumSum;

                if (i == 0)
                {
                    previousCumSum = (seed?.CumSumActiveMinutes ?? 0);
                }
                else
                {
                    previousCumSum = localActivitySummaries[i - 1].CumSumActiveMinutes;
                }

                localActivitySummaries[i].CumSumActiveMinutes = (localActivitySummaries[i].ActiveMinutes) + previousCumSum;
            }

            return localActivitySummaries;
        }

        public IEnumerable<HeartRateSummary> GetCumSums(HeartRateSummary seedHeartSummary, IList<HeartRateSummary> orderedHeartSummaries)
        {
            var localHeartSummaries = orderedHeartSummaries.ToList();

            for (int i = 0; i < localHeartSummaries.Count; i++)
            {
                int? previousCumSum;

                if (i == 0)
                {
                    previousCumSum = (seedHeartSummary?.CumSumCardioAndAbove ?? 0);
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
            Func<T, Decimal?> GetValue,
            Func<T, Decimal?> GetCumSum,
            Action<T, Decimal?> SetCumSum
        ) where T : class
        {
            //there is no point in this local list, each item inside is still modified (they are not copied)
            var localList = orderedList.ToList();

            for (int i = 0; i < localList.Count(); i++)
            {
                Decimal? value = GetValue(localList[i]);

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