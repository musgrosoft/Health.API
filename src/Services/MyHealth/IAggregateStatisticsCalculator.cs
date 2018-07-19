using System;
using System.Collections.Generic;
using Repositories.Models;

namespace Services.MyHealth
{
    public interface IAggregateStatisticsCalculator
    {
        IList<RestingHeartRate> GetMovingAverages(IList<RestingHeartRate> orderedRestingHeartRates, int period);
        IList<Weight> GetMovingAverages(IList<Weight> orderedWeights, int period);
        IList<BloodPressure> GetMovingAverages(IList<BloodPressure> orderedBloodPressures, int period);

        IList<StepCount> GetCumSums(IList<StepCount> orderedStepCounts);
        IList<ActivitySummary> GetCumSums(IList<ActivitySummary> orderedActivitySummaries);
        IList<HeartRateSummary> GetCumSums(IList<HeartRateSummary> orderedHeartSummaries);
        IList<AlcoholIntake> GetCumSums(IList<AlcoholIntake> alcoholIntakes);
    }
}