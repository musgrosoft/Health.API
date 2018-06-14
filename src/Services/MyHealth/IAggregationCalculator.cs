using System;
using System.Collections.Generic;
using Repositories.Models;

namespace Services.MyHealth
{
    public interface IAggregationCalculator
    {
        void SetMovingAveragesOnBloodPressures(IList<BloodPressure> seedBloodPressures, IList<BloodPressure> orderedBloodPressures, int period);
        void SetMovingAveragesOnRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates);
        void SetMovingAveragesOnWeights(IList<Weight> seedWeights, IList<Weight> orderedWeights, int period);

        void SetCumSumsOnStepCounts(int? seed, IList<StepCount> orderedStepCounts);
        void SetCumSumsOnActivitySummaries(int? seed, IList<ActivitySummary> orderedActivitySummaries);
        void SetCumSumsOnHeartSummaries(int? seed, IList<HeartSummary> orderedHeartSummaries);
        void SetCumSumsOnAlcoholIntakes(IList<AlcoholIntake> alcoholIntakes);
    }
}