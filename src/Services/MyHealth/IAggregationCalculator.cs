using System;
using System.Collections.Generic;
using Repositories.Models;

namespace Services.MyHealth
{
    public interface IAggregationCalculator
    {
        void SetMovingAveragesOnBloodPressures(IEnumerable<BloodPressure> bloodPressures);
        void SetMovingAveragesOnRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates);
        void SetMovingAveragesOnWeights(List<Weight> seedWeights, List<Weight> orderedWeights);

        void SetCumSumsOnStepCounts(int? seed, IList<StepCount> orderedStepCounts);
        void SetCumSumsOnActivitySummaries(int? seed, IList<ActivitySummary> orderedActivitySummaries);
        void SetCumSumsOnHeartSummaries(int? seed, List<HeartSummary> orderedHeartSummaries);
        void SetCumSumsOnAlcoholIntakes(IEnumerable<AlcoholIntake> alcoholIntakes);
    }
}