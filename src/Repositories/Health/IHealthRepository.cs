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
        DateTime? GetLatestRunDate();

        void Upsert(Weight weight);
        void Upsert(BloodPressure bloodPressure);
        void Upsert(StepCount stepCount);
        void Upsert(ActivitySummary activitySummary);
        void Upsert(RestingHeartRate restingHeartRate);
        void Upsert(HeartRateSummary heartSummary);

//        void Upsert(HeartRate heartRate);
        void Upsert(Run run);

        void Upsert(AlcoholIntake alcoholIntake);

        //IList<Weight> GetLatestWeights(int number, DateTime beforeDate);
        //IList<HeartRateSummary> GetLatestHeartSummaries(int number, DateTime beforeDate);
        //IEnumerable<BloodPressure> GetLatestBloodPressures(int number, DateTime beforeDate);
        //IEnumerable<RestingHeartRate> GetLatestRestingHeartRates(int number, DateTime beforeDate);
        //IList<StepCount> GetLatestStepCounts(int number, DateTime beforeDate);
        //IList<ActivitySummary> GetLatestActivitySummaries(int number, DateTime beforeDate);


        IList<AlcoholIntake> GetAllAlcoholIntakes();
        IList<Weight> GetAllWeights();
        IList<BloodPressure> GetAllBloodPressures();
        IList<RestingHeartRate> GetAllRestingHeartRates();
        IList<StepCount> GetAllStepCounts();
        IList<ActivitySummary> GetAllActivitySummaries();
        IList<HeartRateSummary> GetAllHeartRateSummaries();

        void Upsert(Ergo ergo);
        IList<Run> GetAllRuns();
        IList<Ergo> GetAllErgos();
        
    }
}