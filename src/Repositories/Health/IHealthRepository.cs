using System;
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
        
        Weight GetLatestWeight();
        BloodPressure GetLatestBloodPressure();
    }
}