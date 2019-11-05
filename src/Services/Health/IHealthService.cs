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
        DateTime GetLatestDrinkDate();
        DateTime GetLatestSleepSummaryDate(DateTime defaultDateTime);
        DateTime GetLatestSleepStateDate(DateTime defaultDateTime);
        DateTime GetLatestExerciseDate(DateTime defaultDateTime);
        

        void UpsertBloodPressures(IEnumerable<BloodPressure> bloodPressures);
        void UpsertWeights(IEnumerable<Weight> weights);
        void UpsertRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates);
        void UpsertAlcoholIntakes(List<Drink> alcoholIntakes);
        void UpsertExercises(List<Exercise> exercises);
        void UpsertSleepSummaries(IEnumerable<SleepSummary> sleepSummaries);
        void UpsertSleepStates(IEnumerable<SleepState> sleepStates);


        List<Weight> GetLatestWeights(int num = 10);
        List<BloodPressure> GetLatestBloodPressures(int num = 10);
        List<RestingHeartRate> GetLatestRestingHeartRates(int num = 10);
        List<Drink> GetLatestDrinks(int num = 10);
        List<Exercise> GetLatestExercises(int num = 10);
        List<SleepSummary> GetLatestSleeps(int num = 10);
        
    }
}