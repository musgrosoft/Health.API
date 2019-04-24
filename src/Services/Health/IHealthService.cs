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
        
        void UpsertBloodPressures(IEnumerable<BloodPressure> bloodPressures);

        void UpsertWeights(IEnumerable<Weight> weights);
        void UpsertRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates);

        void UpsertAlcoholIntakes(List<Drink> alcoholIntakes);

        void UpsertExercises(List<Exercise> exercises);

        

        DateTime GetLatestDrinkDate();
    }
}