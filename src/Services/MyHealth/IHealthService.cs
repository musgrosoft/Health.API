using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Models;

namespace Services.MyHealth
{
    public interface IHealthService
    {
        void AddMovingAveragesToBloodPressures(int period = 10);
        void AddMovingAveragesToRestingHeartRates(int period = 10);

        DateTime GetLatestBloodPressureDate(DateTime defaultDateTime);
        DateTime GetLatestActivitySummaryDate(DateTime defaultDateTime);
        DateTime GetLatestHeartSummaryDate(DateTime defaultDateTime);
        DateTime GetLatestRestingHeartRateDate(DateTime defaultDateTime);
        DateTime GetLatestStepCountDate(DateTime defaultDateTime);
        DateTime GetLatestWeightDate(DateTime defaultDateTime);

        void UpsertBloodPressures(IEnumerable<BloodPressure> bloodPressures);
        void UpsertDailyActivities(IEnumerable<ActivitySummary> activitySummaries);
        void UpsertDailyHeartSummaries(IEnumerable<HeartSummary> heartSummaries);
        void UpsertStepCounts(IEnumerable<StepCount> stepCount);
        void UpsertWeights(IEnumerable<Weight> weights);
        void UpsertRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates);

        void AddMovingAveragesToWeights(int period = 10);
        void CalculateCumSumForStepCounts();
    }
}