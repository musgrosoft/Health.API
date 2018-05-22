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

        DateTime GetLatestBloodPressureDate();
        DateTime GetLatestDailyActivityDate();
        DateTime GetLatestHeartRateDailySummaryDate();
        DateTime GetLatestRestingHeartRateDate();
        DateTime GetLatestStepCountDate();
        DateTime GetLatestWeightDate();

        void UpsertBloodPressures(IEnumerable<BloodPressure> bloodPressures);
        Task UpsertDailyActivities(IEnumerable<ActivitySummary> dailyActivity);
        Task UpsertDailyHeartSummaries(IEnumerable<HeartSummary> heartZoneSummaries);
        Task UpsertStepCounts(IEnumerable<StepCount> stepCount);
        Task UpsertWeights(IEnumerable<Weight> weights);
        Task UpsertRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates);

        Task AddMovingAveragesToWeights(int period = 10);
    }
}