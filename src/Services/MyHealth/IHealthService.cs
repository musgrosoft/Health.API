using System;
using System.Collections.Generic;
using Repositories.Models;

namespace Services.MyHealth
{
    public interface IHealthService
    {
        DateTime GetLatestBloodPressureDate(DateTime defaultDateTime);
        DateTime GetLatestActivitySummaryDate(DateTime defaultDateTime);
        DateTime GetLatestHeartSummaryDate(DateTime defaultDateTime);
        DateTime GetLatestRestingHeartRateDate(DateTime defaultDateTime);
        DateTime GetLatestStepCountDate(DateTime defaultDateTime);
        DateTime GetLatestWeightDate(DateTime defaultDateTime);

        IList<ActivitySummary> GetAllActivitySummaries();
        IList<ActivitySummary> GetAllActivitySummariesByWeek();
        IList<ActivitySummary> GetAllActivitySummariesByMonth();

        void UpsertBloodPressures(IEnumerable<BloodPressure> bloodPressures);
        void UpsertActivitySummaries(IEnumerable<ActivitySummary> activitySummaries);
        void UpsertHeartSummaries(IEnumerable<HeartRateSummary> heartSummaries);
        void UpsertStepCounts(IEnumerable<StepCount> stepCount);
        void UpsertWeights(IEnumerable<Weight> weights);
        void UpsertRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates);
        // void UpsertAlcoholIntakes();
        void UpsertRuns(List<Run> runs);
        void UpsertAlcoholIntakes(List<AlcoholIntake> alcoholIntakes);


        IList<Weight> GetAllWeights();
        IList<BloodPressure> GetAllBloodPressures();
        IList<RestingHeartRate> GetAllRestingHeartRates();
        IList<StepCount> GetAllStepCounts();
        IList<StepCount> GetAllStepCountsByWeek();
        IList<StepCount> GetAllStepCountsByMonth();
        IList<AlcoholIntake> GetAllAlcoholIntakes();
        IList<AlcoholIntake> GetAllAlcoholIntakesByWeek();
        IList<AlcoholIntake> GetAllAlcoholIntakesByMonth();
        IList<HeartRateSummary> GetAllHeartRateSummaries();
        IList<HeartRateSummary> GetAllHeartRateSummariesByWeek();
        IList<HeartRateSummary> GetAllHeartRateSummariesByMonth();

        
    }
}