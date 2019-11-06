﻿using System;
using System.Collections.Generic;
using System.Linq;
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





        public List<RestingHeartRate> GetLatestRestingHeartRates(int num = 10)
        {
            return _healthRepository.GetLatestRestingHeartRate(num);
        }

        public List<Drink> GetLatestDrinks(int num = 10)
        {
            return _healthRepository.GetLatestDrinks(num);
        }

        public DateTime GetLatestSleepSummaryDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestSleepSummaryDate();
            return latestDate ?? defaultDateTime;
        }

        public DateTime GetLatestSleepStateDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestSleepStateDate(defaultDateTime);
            return latestDate ?? defaultDateTime;
        }

        public List<SleepSummary> GetLatestSleeps(int num = 10)
        {
            return _healthRepository.GetLatestSleeps(num);
        }

        public List<Exercise> GetLatestExercises(int num)
        {
            return _healthRepository.GetLatestExercises(num);
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

        public void UpsertWeights(IEnumerable<Weight> weights)
        {
            _healthRepository.Upsert(weights);
        }


        public void UpsertSleepSummaries(IEnumerable<SleepSummary> sleepSummaries)
        {
            foreach (var fitbitSleep in sleepSummaries)
            {
                _healthRepository.Upsert(fitbitSleep);
            }
        }

        public void UpsertSleepStates(IEnumerable<SleepState> sleepStates)
        {
             _healthRepository.Upsert(sleepStates);

            //foreach (var sleepState in sleepStates)
            //{
            //    _healthRepository.Upsert(sleepState);
            //}
        }

        public async Task UpsertBloodPressuresAsync(IEnumerable<BloodPressure> bloodPressures)
        {
            await _healthRepository.UpsertAsync(bloodPressures);
        }

        public void UpsertRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates)
        {
            foreach (var restingHeartRate in restingHeartRates)
            {
                _healthRepository.Upsert(restingHeartRate);
            }
        }

        
        public void UpsertAlcoholIntakes(IEnumerable<Drink> alcoholIntakes)
        {
            _logger.LogMessageAsync($"UNITS : Saving {alcoholIntakes.Count()} alcohol intakes");
            
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
