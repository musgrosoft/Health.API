using System.Collections.Generic;
using System.Linq;
using Repositories.Health;
using Repositories.Models;

namespace Services.Health
{
    public class EntityDecorator : IEntityDecorator
    {
        private readonly IHealthRepository _healthRepository;
        private readonly IAggregateStatisticsCalculator _aggregateStatisticsCalculator;
        private readonly ITargetService _targetService;

        public EntityDecorator(IHealthRepository healthRepository, IAggregateStatisticsCalculator aggregateStatisticsCalculator, ITargetService targetService)
        {
            _healthRepository = healthRepository;
            _aggregateStatisticsCalculator = aggregateStatisticsCalculator;
            _targetService = targetService;
        }


        public IList<Weight> GetAllWeights()
        {
            var allWeights = _healthRepository.GetAllWeights();
            allWeights = _aggregateStatisticsCalculator.GetMovingAverages(allWeights, 10).ToList();
            allWeights = _targetService.SetTargets(allWeights).ToList();

            return allWeights;
        }

        public IList<BloodPressure> GetAllBloodPressures()
        {
            var allBloodPressures = _healthRepository.GetAllBloodPressures();
            allBloodPressures = _aggregateStatisticsCalculator.GetMovingAverages(allBloodPressures, 10);

            return allBloodPressures;
        }

        public IList<RestingHeartRate> GetAllRestingHeartRates()
        {
            var allRestingHeartRates = _healthRepository.GetAllRestingHeartRates();
            allRestingHeartRates = _aggregateStatisticsCalculator.GetMovingAverages(allRestingHeartRates, 10).ToList();

            return allRestingHeartRates;
        }

        public IList<ActivitySummary> GetAllActivitySummaries()
        {
            var allActivitySummaries = _healthRepository.GetAllActivitySummaries().ToList();
            allActivitySummaries = _aggregateStatisticsCalculator.GetCumSums(allActivitySummaries).ToList();
            allActivitySummaries = _targetService.SetTargets(allActivitySummaries).ToList();

            return allActivitySummaries;
        }

        public IList<HeartRateSummary> GetAllHeartRateSummaries()
        {
            var allHeartRateSummaries = _healthRepository.GetAllHeartRateSummaries().ToList();
            allHeartRateSummaries = _aggregateStatisticsCalculator.GetCumSums(allHeartRateSummaries).ToList();
            allHeartRateSummaries = _targetService.SetTargets(allHeartRateSummaries).ToList();

            return allHeartRateSummaries;
        }

        public IList<StepCount> GetAllStepCounts()
        {
            var allStepCounts = _healthRepository.GetAllStepCounts().ToList();
            allStepCounts = _aggregateStatisticsCalculator.GetCumSums(allStepCounts).ToList();
            allStepCounts = _targetService.SetTargets(allStepCounts).ToList();

            return allStepCounts;
        }

        public IList<AlcoholIntake> GetAllAlcoholIntakes()
        {
            var allAlcoholIntakes = _healthRepository.GetAllAlcoholIntakes().ToList();
            allAlcoholIntakes = _aggregateStatisticsCalculator.GetCumSums(allAlcoholIntakes).ToList();
            allAlcoholIntakes = _targetService.SetTargets(allAlcoholIntakes).ToList();

            return allAlcoholIntakes;
        }

    }
}