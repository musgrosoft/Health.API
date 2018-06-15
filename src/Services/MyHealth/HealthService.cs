using System;
using System.Collections.Generic;
using System.Linq;
using Repositories.Health;
using Repositories.Models;
using Utils;

namespace Services.MyHealth
{
    public class HealthService : IHealthService
    {   
        private readonly IConfig _config;
        private readonly ILogger _logger;
        private readonly IHealthRepository _healthRepository;
        private readonly IAggregationCalculator _aggregationCalculator;
        private const int MOVING_AVERAGE_PERIOD = 10;

        public HealthService(
            IConfig config, 
            ILogger logger, 
            IHealthRepository healthRepository,
            IAggregationCalculator aggregationCalculator)
        {
            _config = config;
            _logger = logger;
            _healthRepository = healthRepository;
            _aggregationCalculator = aggregationCalculator;
        }

        public DateTime GetLatestWeightDate(DateTime defaultDateTime)
        {
            var latestWeightDate = _healthRepository.GetLatestWeightDate();
            return latestWeightDate ?? defaultDateTime;
        }

        public DateTime GetLatestBloodPressureDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestBloodPressureDate();
            return latestDate ?? defaultDateTime;
        }
        
        public DateTime GetLatestStepCountDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestStepCountDate();
            return latestDate ?? defaultDateTime;
        }
        
        public DateTime GetLatestActivitySummaryDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestActivitySummaryDate();
            return latestDate ?? defaultDateTime;
        }

        public DateTime GetLatestRestingHeartRateDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestRestingHeartRateDate();
            return latestDate ?? defaultDateTime;
        }

        public DateTime GetLatestHeartSummaryDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestHeartSummaryDate();
            return latestDate ?? defaultDateTime;
        }
        
        public void UpsertWeights(IEnumerable<Weight> weights)
        {
            _logger.Log($"WEIGHT : Saving {weights.Count()} weight");

            var orderedWeights = weights.OrderBy(x => x.DateTime).ToList();

            var previousWeights = _healthRepository.GetLatestWeights(MOVING_AVERAGE_PERIOD - 1, orderedWeights.Min(x => x.DateTime)).ToList();

            _aggregationCalculator.SetMovingAveragesOnWeights(previousWeights, orderedWeights, MOVING_AVERAGE_PERIOD);

            foreach (var weight in weights)
            {
                _healthRepository.Upsert(weight);
            }

        }

        public void UpsertBloodPressures(IEnumerable<BloodPressure> bloodPressures)
        {
            _logger.Log($"BLOOD PRESSURE : Saving {bloodPressures.Count()} blood pressure");

            var orderedBloodPressures = bloodPressures.OrderBy(x => x.DateTime).ToList();

            var previousBloodPressures = _healthRepository.GetLatestBloodPressures(MOVING_AVERAGE_PERIOD-1, orderedBloodPressures.Min(x => x.DateTime)).ToList();

            _aggregationCalculator.SetMovingAveragesOnBloodPressures(previousBloodPressures, orderedBloodPressures, MOVING_AVERAGE_PERIOD);

            foreach (var bloodPressure in bloodPressures)
            {
                _healthRepository.Upsert(bloodPressure);
            }

        }

        public void UpsertRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates)
        {
            _logger.Log($"RESTING HEART RATE : Saving {restingHeartRates.Count()} resting heart rates");

            _logger.Log($"RESTING HEART RATE : Moving averages");

            var orderedRestingHeartRates = restingHeartRates.OrderBy(x => x.DateTime).ToList();

            var previousRestingHeartRates = _healthRepository.GetLatestRestingHeartRates(MOVING_AVERAGE_PERIOD - 1, orderedRestingHeartRates.Min(x=>x.DateTime)).ToList();

            _aggregationCalculator.SetMovingAveragesOnRestingHeartRates(previousRestingHeartRates, orderedRestingHeartRates, MOVING_AVERAGE_PERIOD);

            foreach (var restingHeartRate in restingHeartRates)
            {
                _healthRepository.Upsert(restingHeartRate);
            }
            
        }

        public void UpsertStepCounts(IEnumerable<StepCount> stepCounts)
        {
            _logger.Log($"STEP COUNT : Saving {stepCounts.Count()} Step Count");

            var orderedStepCounts = stepCounts.OrderBy(x => x.DateTime).ToList();

            var previousStepCount = _healthRepository.GetLatestStepCounts(1, orderedStepCounts.Min(x=>x.DateTime)).FirstOrDefault();

            _aggregationCalculator.SetCumSumsOnStepCounts(previousStepCount?.CumSumCount, orderedStepCounts);

            foreach (var stepCount in orderedStepCounts)
            {
                _healthRepository.Upsert(stepCount);
            }
            
        }



        public void UpsertActivitySummaries(IEnumerable<ActivitySummary>  activitySummaries)
        {
            _logger.Log($"ACTIVITY SUMMARY : Saving {activitySummaries.Count()} Activity Summary");

            var orderedActivitySummaries = activitySummaries.OrderBy(x => x.DateTime).ToList();

            var previousActivitySummary = _healthRepository.GetLatestActivitySummaries(1, orderedActivitySummaries.Min(x => x.DateTime)).FirstOrDefault();

            _aggregationCalculator.SetCumSumsOnActivitySummaries(previousActivitySummary?.CumSumActiveMinutes, orderedActivitySummaries);

            foreach (var activitySummary in activitySummaries)
            {
                _healthRepository.Upsert(activitySummary);
            }

        }
        


        public void UpsertHeartSummaries(IEnumerable<HeartSummary> heartSummaries)
        {
            _logger.Log($"HEART SUMMARY : Saving {heartSummaries.Count()} heart summaries");

            var orderedHeartSummaries = heartSummaries.OrderBy(x => x.DateTime).ToList();

            var previousHeartSummary = _healthRepository.GetLatestHeartSummaries(1, orderedHeartSummaries.Min(x => x.DateTime)).FirstOrDefault();

            _aggregationCalculator.SetCumSumsOnHeartSummaries(previousHeartSummary, orderedHeartSummaries);

            foreach (var heartSummary in heartSummaries)
            {
                _healthRepository.Upsert(heartSummary);
            }

        }


        public void UpsertAlcoholIntakes()
        {
            _logger.Log("UNITS : Calculate cum sum");

            var allAlcoholIntakes = _healthRepository.GetAllAlcoholIntakes().ToList();

            _aggregationCalculator.SetCumSumsOnAlcoholIntakes(allAlcoholIntakes);

            foreach (var alcoholIntake in allAlcoholIntakes)
            {
                _healthRepository.Upsert(alcoholIntake);
            }

            //_healthRepository.SaveChanges();


        }


        




    }
}
