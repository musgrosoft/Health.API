using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Health.Models;

namespace Services.Health
{
    public interface IHealthService
    {
        DateTime GetLatestBloodPressureDate(DateTime defaultDateTime);
        DateTime GetLatestRestingHeartRateDate(DateTime defaultDateTime);
        DateTime GetLatestWeightDate(DateTime defaultDateTime);
        DateTime GetLatestDrinkDate(DateTime defaultDateTime);
        DateTime GetLatestSleepSummaryDate(DateTime defaultDateTime);
        DateTime GetLatestSleepStateDate(DateTime defaultDateTime);
        DateTime GetLatestExerciseDate(DateTime defaultDateTime);
        

        Task UpsertBloodPressuresAsync(IEnumerable<BloodPressure> bloodPressures);
        Task UpsertWeights(IEnumerable<Weight> weights);
        Task UpsertRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates);
        Task UpsertDrinksAsync(IEnumerable<Drink> drinks);
        Task UpsertExercisesAsync(List<Exercise> exercises);
        Task UpsertSleepSummaries(IEnumerable<SleepSummary> sleepSummaries);
        Task UpsertSleepStates(IEnumerable<SleepState> sleepStates);


        List<Weight> GetLatestWeights(int num = 10);
        List<BloodPressure> GetLatestBloodPressures(int num = 10);
        List<RestingHeartRate> GetLatestRestingHeartRates(int num = 10);
        List<Drink> GetLatestDrinks(int num = 10);
        List<Exercise> GetLatestExercises(int num = 10);
        List<SleepSummary> GetLatestSleeps(int num = 10);
        
    }
}