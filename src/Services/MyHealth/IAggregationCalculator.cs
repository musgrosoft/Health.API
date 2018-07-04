using System;
using System.Collections.Generic;
using Repositories.Models;

namespace Services.MyHealth
{
    public interface IAggregationCalculator
    {
        // IEnumerable<BloodPressure> GetMovingAverages(IList<BloodPressure> seedBloodPressures, IList<BloodPressure> orderedBloodPressures, int period);
        IList<RestingHeartRate> GetMovingAverages(IList<RestingHeartRate> orderedRestingHeartRates, int period);
        //IList<Weight> GetMovingAverages(IList<Weight> seedWeights, IList<Weight> orderedWeights, int period);
        IList<Weight> GetMovingAverages(IList<Weight> orderedWeights, int period);
        IList<BloodPressure> GetMovingAverages(IList<BloodPressure> orderedBloodPressures, int period);

        IEnumerable<StepCount> GetCumSums(StepCount seed, IList<StepCount> orderedStepCounts);
        IEnumerable<ActivitySummary> GetCumSums(ActivitySummary seed, IList<ActivitySummary> orderedActivitySummaries);
        IEnumerable<HeartRateSummary> GetCumSums(HeartRateSummary seedHeartSummary, IList<HeartRateSummary> orderedHeartSummaries);
        IEnumerable<AlcoholIntake> GetCumSums(IList<AlcoholIntake> alcoholIntakes);
    }
}