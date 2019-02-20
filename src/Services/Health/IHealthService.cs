using System;
using System.Collections.Generic;
using Repositories.Health.Models;

namespace Services.Health
{
    public interface IHealthService
    {
        DateTime GetLatestBloodPressureDate(DateTime defaultDateTime);
        
        
        DateTime GetLatestRestingHeartRateDate(DateTime defaultDateTime);
        
        DateTime GetLatestWeightDate(DateTime defaultDateTime);
        //DateTime GetLatestRunDate(DateTime defaultDateTime);
       // DateTime GetLatestDetailedHeartRatesDate(DateTime defaultDateTime);

        //IList<ActivitySummary> GetAllActivitySummaries();
        //IList<ActivitySummary> GetAllActivitySummariesByWeek();
        //IList<ActivitySummary> GetAllActivitySummariesByMonth();

        void UpsertBloodPressures(IEnumerable<BloodPressure> bloodPressures);

        void UpsertWeights(IEnumerable<Weight> weights);
        void UpsertRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates);
        // void UpsertAlcoholIntakes();
        //void UpsertRuns(IEnumerable<Run> runs);
        void UpsertAlcoholIntakes(List<AlcoholIntake> alcoholIntakes);

        void UpsertExercises(List<Exercise> exercises);

        //IList<Weight> GetAllWeights();
        //IList<BloodPressure> GetAllBloodPressures();
        //IList<RestingHeartRate> GetAllRestingHeartRates();
        //IList<StepCount> GetAllStepCounts();
        //IList<StepCount> GetAllStepCountsByWeek();
        //IList<StepCount> GetAllStepCountsByMonth();
        //IList<AlcoholIntake> GetAllAlcoholIntakes();
        //IList<AlcoholIntake> GetAllAlcoholIntakesByWeek();
        //IList<AlcoholIntake> GetAllAlcoholIntakesByMonth();
        //IList<HeartRateSummary> GetAllHeartRateSummaries();
        //IList<HeartRateSummary> GetAllHeartRateSummariesByWeek();
        //IList<HeartRateSummary> GetAllHeartRateSummariesByMonth();


        //void UpsertErgos(List<Ergo> rows);
        //IList<Run> GetAllRuns();
        //IList<Ergo> GetAllErgos();

        //void UpsertHeartRates(List<HeartRate> detailedHeartRates);

        
    }
}