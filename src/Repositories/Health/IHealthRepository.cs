using System;
using Repositories.Health.Models;

namespace Repositories.Health
{
    public interface IHealthRepository
    {
        DateTime? GetLatestBloodPressureDate();
        DateTime? GetLatestWeightDate();
        DateTime? GetLatestRestingHeartRateDate();
        
        void Upsert(Weight weight);
        void Upsert(BloodPressure bloodPressure);
        void Upsert(RestingHeartRate restingHeartRate);
        void Upsert(AlcoholIntake alcoholIntake);
        void Upsert(Exercise exercise);

    }
}