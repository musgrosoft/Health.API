using System;
using System.Collections.Generic;
using Repositories.Health.Models;

namespace Repositories.Health
{
    public interface IHealthRepository
    {
        DateTime? GetLatestStepCountDate();
        DateTime? GetLatestBloodPressureDate();
        DateTime? GetLatestWeightDate();
        DateTime? GetLatestActivitySummaryDate();
        DateTime? GetLatestRestingHeartRateDate();
        DateTime? GetLatestHeartSummaryDate();
        DateTime? GetLatestRunDate();
        DateTime? GetLatestDetailedHeartRatesDate(string source);


        void Upsert(Weight weight);
        void Upsert(BloodPressure bloodPressure);
        void Upsert(StepCount stepCount);
        void Upsert(ActivitySummary activitySummary);
        void Upsert(RestingHeartRate restingHeartRate);
        void Upsert(HeartRateSummary heartSummary);
        void Upsert(Run run);
        void Upsert(AlcoholIntake alcoholIntake);
        void Upsert(Ergo ergo);

        void Upsert(HeartRate detailedHeartRate);
        void UpsertMany(IEnumerable<HeartRate> detailedHeartRates);
    }
}