using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Models;

namespace Services.MyHealth
{
    public interface IHealthService
    {
        void AddMovingAveragesToBloodPressures(int period = 10);
        Task AddMovingAveragesToRestingHeartRates(int period = 10);

        DateTime GetLatestBloodPressureDate(DateTime defaultDateTime);
        DateTime GetLatestActivitySummaryDate(DateTime defaultDateTime);
        DateTime GetLatestHeartSummaryDate(DateTime defaultDateTime);
        DateTime GetLatestRestingHeartRateDate(DateTime defaultDateTime);
        DateTime GetLatestStepCountDate(DateTime defaultDateTime);
        DateTime GetLatestWeightDate(DateTime defaultDateTime);

        Task UpsertBloodPressures(IEnumerable<BloodPressure> bloodPressures);
        Task UpsertDailyActivities(IEnumerable<ActivitySummary> activitySummaries);
        Task UpsertDailyHeartSummaries(IEnumerable<HeartSummary> heartSummaries);
        void UpsertStepCounts(IEnumerable<StepCount> stepCount);
        void UpsertWeights(IEnumerable<Weight> weights);
        Task UpsertRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates);

        void AddMovingAveragesToWeights(int period = 10);
    }
}