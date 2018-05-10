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

        Task UpsertBloodPressures(IEnumerable<BloodPressure> bloodPressures);
        Task UpsertDailyActivity(DailyActivity dailyActivity);
        Task UpsertDailyHeartSummaries(IEnumerable<HeartRateZoneSummary> heartZoneSummaries);
        Task UpsertRestingHeartRate(RestingHeartRate restingHeartRate);
        Task UpsertStepCounts(IEnumerable<StepCount> stepCount);
        Task UpsertWeights(IEnumerable<Weight> weights);
    }
}