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
        void UpsertActivitySummaries(IEnumerable<ActivitySummary> activitySummaries);
        void UpsertHeartSummaries(IEnumerable<HeartSummary> heartSummaries);
        void UpsertStepCounts(IEnumerable<StepCount> stepCount);
        void UpsertWeights(IEnumerable<Weight> weights);
        void UpsertRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates);

        void AddMovingAveragesToWeights(int period = 10);

        void CalculateCumSumForStepCounts();
        void CalculateCumSumForUnits();
        void CalculateCumSumForActivitySummaries();
        void CalculateCumSumForHeartSummaries();
    }
}