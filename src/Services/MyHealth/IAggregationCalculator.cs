using System;
using System.Collections.Generic;
using Repositories.Models;

namespace Services.MyHealth
{
    public interface IAggregationCalculator
    {
        IList<RestingHeartRate> GetMovingAverages(IList<RestingHeartRate> orderedRestingHeartRates, int period);
        IList<Weight> GetMovingAverages(IList<Weight> orderedWeights, int period);
        IList<BloodPressure> GetMovingAverages(IList<BloodPressure> orderedBloodPressures, int period);

        IEnumerable<StepCount> GetCumSums(IList<StepCount> orderedStepCounts);
        IEnumerable<ActivitySummary> GetCumSums(IList<ActivitySummary> orderedActivitySummaries);
        IEnumerable<HeartRateSummary> GetCumSums(IList<HeartRateSummary> orderedHeartSummaries);
        IEnumerable<AlcoholIntake> GetCumSums(IList<AlcoholIntake> alcoholIntakes);
    }
}