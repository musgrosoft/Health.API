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
        DateTime GetLatestTargetDate(DateTime defaultDateTime);


        Task UpsertAsync(IEnumerable<BloodPressure> bloodPressures);
        Task UpsertAsync(IEnumerable<Weight> weights);
        Task UpsertAsync(IEnumerable<RestingHeartRate> restingHeartRates);
        Task UpsertAsync(IEnumerable<Drink> drinks);
        Task UpsertAsync(IEnumerable<Exercise> exercises);
        Task UpsertAsync(IEnumerable<SleepSummary> sleepSummaries);
        Task UpsertAsync(IEnumerable<SleepState> sleepStates);
        Task UpsertAsync(IEnumerable<Target> targets);


        //List<Weight> GetLatestWeights(int num = 10);
        //List<BloodPressure> GetLatestBloodPressures(int num = 10);
        //List<RestingHeartRate> GetLatestRestingHeartRates(int num = 10);
        //List<Drink> GetLatestDrinks(int num = 10);
        //List<Exercise> GetLatestExercises(int num = 10);
        //List<SleepSummary> GetLatestSleeps(int num = 10);

    }
}