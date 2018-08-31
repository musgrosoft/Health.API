using System;
using System.Collections.Generic;
using System.Linq;
using Repositories.Health;
using Repositories.Models;
using Utils;

namespace Services.Health
{
    public class HealthService : IHealthService
    {   
        private readonly IConfig _config;
        private readonly ILogger _logger;
        private readonly IHealthRepository _healthRepository;
        private readonly IEntityAggregator _entityAggregator;
        private readonly IEntityDecorator _entityDecorator;
        private const int MOVING_AVERAGE_PERIOD = 10;

        public HealthService(IConfig config,
            ILogger logger,
            IHealthRepository healthRepository,
            IEntityAggregator entityAggregator, 
            IEntityDecorator entityDecorator)
        {
            _config = config;
            _logger = logger;
            _healthRepository = healthRepository;
            _entityAggregator = entityAggregator;
            _entityDecorator = entityDecorator;
        }


        public IList<Run> GetAllRuns()
        {
            return _healthRepository.GetAllRuns();
        }

        public IList<Ergo> GetAllErgos()
        {
            return _healthRepository.GetAllErgos();
        }

        public IList<Weight> GetAllWeights()
        {
            return _entityDecorator.GetAllWeights();
        }

        public IList<BloodPressure> GetAllBloodPressures()
        {
            return _entityDecorator.GetAllBloodPressures();
        }

        public IList<RestingHeartRate> GetAllRestingHeartRates()
        {
            return _entityDecorator.GetAllRestingHeartRates();
        }

        public IList<StepCount> GetAllStepCounts()
        {
            return _entityDecorator.GetAllStepCounts();
        }

        public IList<StepCount> GetAllStepCountsByWeek()
        {
            var dailyStepCounts = _entityDecorator.GetAllStepCounts();

            var weeklyStepCounts = _entityAggregator.GroupByWeek(dailyStepCounts);

            return weeklyStepCounts;
        }

        public IList<StepCount> GetAllStepCountsByMonth()
        {
            var dailyStepCounts = _entityDecorator.GetAllStepCounts();

            var monthlyStepCounts = _entityAggregator.GroupByMonth(dailyStepCounts);

            return monthlyStepCounts;
        }

        public IList<AlcoholIntake> GetAllAlcoholIntakes()
        {
            return _entityDecorator.GetAllAlcoholIntakes();
        }

        public IList<AlcoholIntake> GetAllAlcoholIntakesByWeek()
        {
            var dailyAlcoholIntakes = _entityDecorator.GetAllAlcoholIntakes();

            var weeklyAlcoholIntakes = _entityAggregator.GroupByWeek(dailyAlcoholIntakes);

            return weeklyAlcoholIntakes;
        }

        public IList<AlcoholIntake> GetAllAlcoholIntakesByMonth()
        {
            var dailyAlcoholIntakes = _entityDecorator.GetAllAlcoholIntakes();

            var monthlyAlcoholIntakes = _entityAggregator.GroupByMonth(dailyAlcoholIntakes);

            return monthlyAlcoholIntakes;
        }

        public IList<HeartRateSummary> GetAllHeartRateSummaries()
        {
            return _entityDecorator.GetAllHeartRateSummaries();
        }

        public IList<HeartRateSummary> GetAllHeartRateSummariesByWeek()
        {
            var dailyHeartRateSummaries = _entityDecorator.GetAllHeartRateSummaries();

            var weeklyHeartRateSummaries = _entityAggregator.GroupByWeek(dailyHeartRateSummaries);

            return weeklyHeartRateSummaries;
        }

        public IList<HeartRateSummary> GetAllHeartRateSummariesByMonth()
        {
            var dailyHeartRateSummaries = _entityDecorator.GetAllHeartRateSummaries();

            var monthlyHeartRateSummaries = _entityAggregator.GroupByMonth(dailyHeartRateSummaries);

            return monthlyHeartRateSummaries;
        }


        public IList<ActivitySummary> GetAllActivitySummaries()
        {
            return _entityDecorator.GetAllActivitySummaries();
        }

        public IList<ActivitySummary> GetAllActivitySummariesByWeek()
        {
            var dailyActivitySummaries = _entityDecorator.GetAllActivitySummaries();

            var weeklyActivitySummaries = _entityAggregator.GroupByWeek(dailyActivitySummaries);

            return weeklyActivitySummaries;
        }

        public IList<ActivitySummary> GetAllActivitySummariesByMonth()
        {
            var dailyActivitySummaries = _entityDecorator.GetAllActivitySummaries();

            var monthlyActivitySummaries = _entityAggregator.GroupByMonth(dailyActivitySummaries);

            return monthlyActivitySummaries;
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

            _logger.LogMessageAsync($"WEIGHT : Saving {enumerable.Count()} weight");

            foreach (var weight in enumerable)
            {
                _healthRepository.Upsert(weight);
            }
        }

        public void UpsertBloodPressures(IEnumerable<BloodPressure> bloodPressures)
        {
            _logger.LogMessageAsync($"BLOOD PRESSURE : Saving {bloodPressures.Count()} blood pressure");

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

        public void UpsertStepCounts(IEnumerable<StepCount> stepCounts)
        {
            _logger.LogMessageAsync($"STEP COUNT : Saving {stepCounts.Count()} Step Count");
            
            foreach (var stepCount in stepCounts)
            {
                _healthRepository.Upsert(stepCount);
            }            
        }
        
        public void UpsertActivitySummaries(IEnumerable<ActivitySummary>  activitySummaries)
        {
            _logger.LogMessageAsync($"ACTIVITY SUMMARY : Saving {activitySummaries.Count()} Activity Summary");

            foreach (var activitySummary in activitySummaries)
            {
                _healthRepository.Upsert(activitySummary);
            }
        }
        
        public void UpsertHeartSummaries(IEnumerable<HeartRateSummary> heartSummaries)
        {
            _logger.LogMessageAsync($"HEART SUMMARY : Saving {heartSummaries.Count()} heart summaries");

            foreach (var heartSummary in heartSummaries)
            {
                _healthRepository.Upsert(heartSummary);
            }
        }

        public void UpsertRuns(List<Run> runs)
        {
            _logger.LogMessageAsync($"RUN : Saving {runs.Count()} runs");

            foreach (var run in runs)
            {
                _healthRepository.Upsert(run);
            }
        }

        public void UpsertErgos(List<Ergo> rows)
        {
            _logger.LogMessageAsync($"ROWS : Saving {rows.Count()} rows");

            foreach (var row in rows)
            {
                _healthRepository.Upsert(row);
            }
        }

        public void UpsertAlcoholIntakes(List<AlcoholIntake> alcoholIntakes)
        {
            _logger.LogMessageAsync($"UNITS : Saving {alcoholIntakes.Count} alcohol intakes");
            
            foreach (var alcoholIntake in alcoholIntakes)
            {
                _healthRepository.Upsert(alcoholIntake);
            }
        }

    }
}
