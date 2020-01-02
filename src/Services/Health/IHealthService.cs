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
        DateTime GetLatestExerciseDate(DateTime defaultDateTime);


        Task UpsertAsync(IEnumerable<BloodPressure> bloodPressures);
        Task UpsertAsync(IEnumerable<Weight> weights);
        Task UpsertAsync(IEnumerable<RestingHeartRate> restingHeartRates);
        Task UpsertAsync(IEnumerable<Drink> drinks);
        Task UpsertAsync(IEnumerable<Exercise> exercises);
        Task UpsertAsync(IEnumerable<SleepSummary> sleepSummaries);
        Task UpsertAsync(IEnumerable<Target> targets);



    }
}