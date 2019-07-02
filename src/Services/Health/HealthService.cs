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

        public Target GetTarget(DateTime date)
        {
            return _healthRepository.GetTarget(date);
        }

        public List<Weight> GetLatestWeights(int num = 10)
        {
            return _healthRepository.GetLatestWeights(num);
        }

        public List<BloodPressure> GetLatestBloodPressures(int num = 10)
        {
            return _healthRepository.GetLatestBloodPressures(num);
        }

               
        public DateTime GetLatestExerciseDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestExerciseDate();
            return latestDate ?? defaultDateTime;
        }

        public List<RestingHeartRate> GetLatestRestingHeartRates(int num = 10)
        {
            return _healthRepository.GetLatestRestingHeartRate(num);
        }

        public List<Drink> GetLatestDrinks(int num = 10)
        {
            return _healthRepository.GetLatestDrinks(num);
        }

        public List<Exercise> GetLatestExercises(int num)
        {
            return _healthRepository.GetLatestExercises(num);
        }

        public Exercise GetFurthest15MinuteErgo(DateTime fromDate)
        {
            return _healthRepository.GetFurthest(fromDate, "ergo", 900);
        }

        public Exercise GetFurthest30MinuteTreadmill(DateTime fromDate)
        {
            return _healthRepository.GetFurthest(fromDate, "treadmill", 1800);
        }

        public double GetCumSumUnits()
        {
            return _healthRepository.GetCumSumUnits();
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
