using System;
using System.Collections.Generic;
using Repositories.Health.Models;

namespace Repositories.Health
{
    public interface IHealthRepository
    {
        DateTime? GetLatestBloodPressureDate();
        DateTime? GetLatestWeightDate();
        DateTime? GetLatestRestingHeartRateDate();
        DateTime? GetLatestDrinkDate();

        void Upsert(Weight weight);
        void Upsert(BloodPressure bloodPressure);
        void Upsert(RestingHeartRate restingHeartRate);
        void Upsert(Drink drink);
        void Upsert(Exercise exercise);
        
        List<Weight> GetLatestWeights(int num);
        List<BloodPressure> GetLatestBloodPressures(int num);
        DateTime? GetLatestExerciseDate();
        List<RestingHeartRate> GetLatestExercises(int num);
    }
}