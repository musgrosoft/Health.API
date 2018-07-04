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
        private readonly ITargetService _targetService;
        private const int MOVING_AVERAGE_PERIOD = 10;

        public HealthService(
            IConfig config, 
            ILogger logger, 
            IHealthRepository healthRepository,
            IAggregationCalculator aggregationCalculator, 
            ITargetService targetService)
        {
            _config = config;
            _logger = logger;
            _healthRepository = healthRepository;
            _aggregationCalculator = aggregationCalculator;
            _targetService = targetService;
        }

        public IList<Weight> GetAllWeights()
        {
            var allWeights = _healthRepository.GetAllWeights()
                                .GroupBy(x => x.CreatedDate.Date)
                                .Select(g => new Weight
                                {
                                    CreatedDate = g.Key.Date,
                                    Kg = g.Average(w => w.Kg)
                                })
                                .OrderBy(x => x.CreatedDate).ToList();

            allWeights = _aggregationCalculator.GetMovingAverages(allWeights, 10).ToList();
            allWeights = _targetService.SetTargetWeights(allWeights,365).ToList();            

            return allWeights;
        }

        public IList<BloodPressure> GetAllBloodPressures()
        {
            var allBloodPressures = _healthRepository.GetAllBloodPressures();
            allBloodPressures = _aggregationCalculator.GetMovingAverages(allBloodPressures, 10);
           // allBloodPressures = _targetService.SetTargetBloodPressures(allBloodPressures, 365);

            return allBloodPressures;
        }

        public IList<RestingHeartRate> GetAllRestingHeartRates()
        {
            var allRestingHeartRates = _healthRepository.GetAllRestingHeartRates().OrderBy(x=>x.CreatedDate).ToList();
            allRestingHeartRates = _aggregationCalculator.GetMovingAverages(allRestingHeartRates, 10).ToList();
            
            return allRestingHeartRates;
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
            var enumerable = weights.ToList();

            _logger.Log($"WEIGHT : Saving {enumerable.Count()} weight");

            foreach (var weight in enumerable)
            {
                _healthRepository.Upsert(weight);
            }
        }

        public void UpsertBloodPressures(IEnumerable<BloodPressure> bloodPressures)
        {
            _logger.Log($"BLOOD PRESSURE : Saving {bloodPressures.Count()} blood pressure");

            foreach (var bloodPressure in bloodPressures)
            {
                _healthRepository.Upsert(bloodPressure);
            }
        }

        public void UpsertRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates)
        {
            _logger.Log($"RESTING HEART RATE : Saving {restingHeartRates.Count()} resting heart rates");

            foreach (var restingHeartRate in restingHeartRates)
            {
                _healthRepository.Upsert(restingHeartRate);
            }
        }

        public void UpsertStepCounts(IEnumerable<StepCount> stepCounts)
        {
            _logger.Log($"STEP COUNT : Saving {stepCounts.Count()} Step Count");

            var orderedStepCounts = stepCounts.OrderBy(x => x.CreatedDate).ToList();

            var previousStepCount = _healthRepository.GetLatestStepCounts(1, orderedStepCounts.Min(x=>x.CreatedDate)).FirstOrDefault();

            var stepCountWithSums = _aggregationCalculator.GetCumSums(previousStepCount, orderedStepCounts);

            foreach (var stepCount in stepCountWithSums)
            {
                _healthRepository.Upsert(stepCount);
            }            
        }
        
        public void UpsertActivitySummaries(IEnumerable<ActivitySummary>  activitySummaries)
        {
            _logger.Log($"ACTIVITY SUMMARY : Saving {activitySummaries.Count()} Activity Summary");

            var orderedActivitySummaries = activitySummaries.OrderBy(x => x.CreatedDate).ToList();

            var previousActivitySummary = _healthRepository.GetLatestActivitySummaries(1, orderedActivitySummaries.Min(x => x.CreatedDate)).FirstOrDefault();

            var activitySummariesWithSums = _aggregationCalculator.GetCumSums(previousActivitySummary, orderedActivitySummaries);

            foreach (var activitySummary in activitySummariesWithSums)
            {
                _healthRepository.Upsert(activitySummary);
            }
        }
        
        public void UpsertHeartSummaries(IEnumerable<HeartRateSummary> heartSummaries)
        {
            _logger.Log($"HEART SUMMARY : Saving {heartSummaries.Count()} heart summaries");

            var orderedHeartSummaries = heartSummaries.OrderBy(x => x.CreatedDate).ToList();

            var previousHeartSummary = _healthRepository.GetLatestHeartSummaries(1, orderedHeartSummaries.Min(x => x.CreatedDate)).FirstOrDefault();

            var heartSummariesWithSums = _aggregationCalculator.GetCumSums(previousHeartSummary, orderedHeartSummaries);

            foreach (var heartSummary in heartSummariesWithSums)
            {
                _healthRepository.Upsert(heartSummary);
            }
        }
        
        public void UpsertAlcoholIntakes()
        {
            _logger.Log("UNITS : Calculate cum sum");

            var allAlcoholIntakes = _healthRepository.GetAllAlcoholIntakes().ToList();

            //get these from sheet instead of my own db
             var localAlc = allAlcoholIntakes.Select(x=>new AlcoholIntake {CreatedDate=x.CreatedDate,Units=x.Units,CumSumUnits=x.CumSumUnits }).OrderBy(x=>x.CreatedDate).ToList();

            var alcoholIntakesWithSums = _aggregationCalculator.GetCumSums(localAlc);

            foreach (var alcoholIntake in localAlc)
            {
                _healthRepository.Upsert(alcoholIntake);
            }
        }
        
    }
}
