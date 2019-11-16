using System;
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

//        public Target GetTarget(DateTime date)
//        {
//            return _healthRepository.GetTarget(date);
//        }




        //public List<Weight> GetLatestWeights(int num = 10)
        //{
        //    return _healthRepository.GetLatestWeights(num);
        //}

        //public List<BloodPressure> GetLatestBloodPressures(int num = 10)
        //{
        //    return _healthRepository.GetLatestBloodPressures(num);
        //}





        //public List<RestingHeartRate> GetLatestRestingHeartRates(int num = 10)
        //{
        //    return _healthRepository.GetLatestRestingHeartRate(num);
        //}

        //public List<Drink> GetLatestDrinks(int num = 10)
        //{
        //    return _healthRepository.GetLatestDrinks(num);
        //}

        public DateTime GetLatestSleepSummaryDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestSleepSummaryDate();
            return latestDate ?? defaultDateTime;
        }

        public DateTime GetLatestSleepStateDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestSleepStateDate();
            return latestDate ?? defaultDateTime;
        }

        //public List<SleepSummary> GetLatestSleeps(int num = 10)
        //{
        //    return _healthRepository.GetLatestSleeps(num);
        //}

        //public List<Exercise> GetLatestExercises(int num)
        //{
        //    return _healthRepository.GetLatestExercises(num);
        //}



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

        public async Task UpsertAsync(IEnumerable<Weight> weights)
        {
            await _healthRepository.UpsertAsync(weights);
        }


        public async Task UpsertAsync(IEnumerable<SleepSummary> sleepSummaries)
        {
            await _healthRepository.UpsertAsync(sleepSummaries);
        }

        public async Task UpsertAsync(IEnumerable<SleepState> sleepStates)
        {
             await _healthRepository.UpsertAsync(sleepStates);
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

        public async Task UpsertAsync(List<Exercise> exercises)
        {
            await _healthRepository.UpsertAsync(exercises);
        }



    }
}
