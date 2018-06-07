using System;
using System.Collections.Generic;
using Repositories.Models;

namespace Repositories.Health
{
    public interface IHealthRepository
    {
        void Insert<T>(T obj) where T : class;
        
        DateTime? GetLatestStepCountDate();
        DateTime? GetLatestBloodPressureDate();
        DateTime? GetLatestWeightDate();
        DateTime? GetLatestActivitySummaryDate();
        DateTime? GetLatestRestingHeartRateDate();
        DateTime? GetLatestHeartSummaryDate();

        Weight Find(Weight weight);
        BloodPressure Find(BloodPressure bloodPressure);
        StepCount Find(StepCount stepCount);
        ActivitySummary Find(ActivitySummary activitySummary);
        RestingHeartRate Find(RestingHeartRate restingHeartRate);
        HeartSummary Find(HeartSummary heartSummary);

        void Update(Weight existingWeight, Weight newWeight);
        void Update(BloodPressure existingBloodPressure, BloodPressure bloodPressure);
        void Update(StepCount existingStepCount, StepCount stepCount);
        void Update(ActivitySummary existingActivitySummary, ActivitySummary activitySummary);
        void Update(RestingHeartRate existingRestingHeartRate, RestingHeartRate restingHeartRate);
        void Update(HeartSummary existingHeartSummary, HeartSummary heartSummary);
        IEnumerable<Weight> GetLatestWeights(int number);
        IEnumerable<HeartSummary> GetLatestHeartSummaries(int number);
        IEnumerable<BloodPressure> GetLatestBloodPressures(int number);
        IEnumerable<RestingHeartRate> GetLatestRestingHeartRates(int number);
        IEnumerable<StepCount> GetLatestStepCounts(int number);
        IEnumerable<ActivitySummary> GetLatestActivitySummaries(int number);
        IEnumerable<AlcoholIntake> GetAllAlcoholIntakes();
        void SaveChanges();
    }
}