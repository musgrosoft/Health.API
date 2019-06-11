using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<Weight> GetLatestWeights(int num = 10)
        {
            return _healthRepository.GetLatestWeights(num);
        }

        public List<BloodPressure> GetLatestBloodPressures(int num = 10)
        {
            return _healthRepository.GetLatestBloodPressures(num);
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

        public DateTime GetLatestDrinkDate()
        {
            var latestDate = _healthRepository.GetLatestDrinkDate();
            return latestDate ?? DateTime.MinValue;
        }


        public void UpsertWeights(IEnumerable<Weight> weights)
        {
            var enumerable = weights.ToList();

            foreach (var weight in enumerable)
            {
                _healthRepository.Upsert(weight);
            }
        }

        public void UpsertBloodpressures(IEnumerable<BloodPressure> bloodPressures)
        {
            foreach (var bloodPressure in bloodPressures)
            {
                _healthRepository.Upsert(bloodPressure);
            }
        }

        public void UpsertRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates)
        {
            _logger.LogMessageAsync($"RESTING HEART RATE : Saving {restingHeartRates.Count()} resting heart rates");

            foreach (var restingHeartRate in restingHeartRates)
            {
                _healthRepository.Upsert(restingHeartRate);
            }
        }

        
        public void UpsertAlcoholIntakes(List<Drink> alcoholIntakes)
        {
            _logger.LogMessageAsync($"UNITS : Saving {alcoholIntakes.Count} alcohol intakes");
            
            foreach (var alcoholIntake in alcoholIntakes)
            {
                _healthRepository.Upsert(alcoholIntake);
            }
        }

        public void UpsertExercises(List<Exercise> exercises)
        {
            _logger.LogMessageAsync($"EXERCISES : Saving {exercises.Count} exercises");

            foreach (var exercise in exercises)
            {
                _healthRepository.Upsert(exercise);
            }
        }



    }
}
