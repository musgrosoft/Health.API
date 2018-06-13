using System;
using System.Collections.Generic;
using Repositories.Models;

namespace Services.MyHealth
{
    public interface IAggregationCalculator
    {
        // IList<int?> GenerateCumSums(object stepCounts, Func<object, object> func, Func<object, object> func1, Func<object, object> func2, Func<object, object, object> func3);


        void AddMovingAverageTo<T>(
            IEnumerable<T> theList,
            Func<T, DateTime> dateTimeSelector,
            Func<T, Decimal> GetValue,
            Action<T, Decimal?> SetMovingAverage,
            int period = 10
        ) where T : class;

        void CalculateCumSumFor<T>(
            IEnumerable<T> theList,
            Func<T, DateTime> dateTimeSelector,
            Func<T, Decimal?> GetValue,
            Func<T, Decimal?> GetCumSum,
            Action<T, Decimal?> SetCumSum
        ) where T : class;

        void SetMovingAveragesToBloodPressures(IEnumerable<BloodPressure> bloodPressures, List<Weight> orderedWeights, int period);
        void AddMovingAveragesToRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates);
        void SetMovingAveragesForWeights(List<Weight> seedWeights, List<Weight> orderedWeights, int period);
        void SetMovingAveragesFor<T>(List<T> seedTs, List<T> orderedTs, Func<T, Decimal?> getValue, Action<T, Decimal?> setValue, int period = 10);
        void SetCumSumsForStepCounts(int? seed, IList<StepCount> orderedStepCounts);
        void SetCumSumsForActivitySummaries(int? seed, IList<ActivitySummary> orderedActivitySummaries);
        void SetCumSumsForHeartSummaries(int? seed, List<HeartSummary> orderedHeartSummaries);
        void AddCumSumsToAlcoholIntakes(IEnumerable<AlcoholIntake> alcoholIntakes);
    }
}