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

        void UpsertBloodpressures(IEnumerable<BloodPressure> bloodPressures);
        void UpsertWeights(IEnumerable<Weight> weights);
        void UpsertRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates);
        void UpsertAlcoholIntakes(List<Drink> alcoholIntakes);
        void UpsertExercises(List<Exercise> exercises);


        List<Weight> GetLatestWeights(int num = 10);
        List<BloodPressure> GetLatestBloodPressures(int num = 10);
        DateTime GetLatestExerciseDate(DateTime defaultDateTime);
        List<RestingHeartRate> GetLatestRestingHeartRates(int num = 10);
        List<Drink> GetLatestDrinks(int num = 10);
        List<Exercise> GetLatestExercises(int num = 10);
    }
}