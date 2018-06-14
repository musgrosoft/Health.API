using System;
using System.Collections.Generic;
using Repositories.Models;

namespace Repositories.Health
{
    public interface IHealthRepository
    {
    //    void Insert<T>(T obj) where T : class;
        
        DateTime? GetLatestStepCountDate();
        DateTime? GetLatestBloodPressureDate();
        DateTime? GetLatestWeightDate();
        DateTime? GetLatestActivitySummaryDate();
        DateTime? GetLatestRestingHeartRateDate();
        DateTime? GetLatestHeartSummaryDate();
        
        void Upsert(Weight weight);
        void Upsert(BloodPressure bloodPressure);
        void Upsert(StepCount stepCount);
        void Upsert(ActivitySummary activitySummary);
        void Upsert(RestingHeartRate restingHeartRate);
        void Upsert(HeartSummary heartSummary);

        IList<Weight> GetLatestWeights(int number, DateTime beforeDate);
        IList<HeartSummary> GetLatestHeartSummaries(int number, DateTime beforeDate);
        IEnumerable<BloodPressure> GetLatestBloodPressures(int number, DateTime beforeDate);
        IEnumerable<RestingHeartRate> GetLatestRestingHeartRates(int number);
        IList<StepCount> GetLatestStepCounts(int number, DateTime beforeDate);
        IList<ActivitySummary> GetLatestActivitySummaries(int number, DateTime beforeDate);
        IEnumerable<AlcoholIntake> GetAllAlcoholIntakes();
        
    }
}