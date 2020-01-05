using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Health;
using Repositories.Health.Models;
using Utils;

namespace Services.Health
{
    public class HealthService : IHealthService
    {   
        private readonly ILogger _logger;
        private readonly IHealthRepository _healthRepository;
        
        public HealthService(
            ILogger logger,
            IHealthRepository healthRepository)
        {
            _logger = logger;
            _healthRepository = healthRepository;
        }

        public DateTime GetLatestWeightDate(DateTime defaultDateTime)
        {
            var latestWeightDate = _healthRepository.GetLatestWeightDate();
            return latestWeightDate ?? defaultDateTime;
        }

        public DateTime GetLatestSleepSummaryDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestSleepSummaryDate();
            return latestDate ?? defaultDateTime;
        }

        public DateTime GetLatestBloodPressureDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestBloodPressureDate();
            return latestDate ?? defaultDateTime;
        }
       
        public DateTime GetLatestRestingHeartRateDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestRestingHeartRateDate();
            return latestDate ?? defaultDateTime;
        }

        public DateTime GetLatestDrinkDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestDrinkDate();
            return latestDate ?? defaultDateTime;
        }


        public DateTime GetLatestExerciseDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestExerciseDate();
            return latestDate ?? defaultDateTime;
        }

        public DateTime GetLatestTargetDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestTargetDate();
            return latestDate ?? defaultDateTime;
        }

        public async Task UpsertAsync(IEnumerable<Weight> weights)
        {
            await _healthRepository.UpsertAsync(weights);
        }


        public async Task UpsertAsync(IEnumerable<SleepSummary> sleepSummaries)
        {
            await _healthRepository.UpsertAsync(sleepSummaries);
        }


        public async Task UpsertAsync(IEnumerable<Target> targets)
        {
            await _healthRepository.UpsertAsync(targets);
        }

        public async Task UpsertAsync(IEnumerable<BloodPressure> bloodPressures)
        {
            await _healthRepository.UpsertAsync(bloodPressures);
        }

        public async Task UpsertAsync(IEnumerable<RestingHeartRate> restingHeartRates)
        {
            await _healthRepository.UpsertAsync(restingHeartRates);
        }

        
        public async Task UpsertAsync(IEnumerable<Drink> drinks)
        {
            await _healthRepository.UpsertAsync(drinks);
        }

        public async Task UpsertAsync(IEnumerable<Exercise> exercises)
        {
            await _healthRepository.UpsertAsync(exercises);
        }



    }
}
