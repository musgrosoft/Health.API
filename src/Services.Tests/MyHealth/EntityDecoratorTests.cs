using System;
using System.Collections.Generic;
using Moq;
using Repositories.Health;
using Repositories.Models;
using Services.Health;
using Utils;
using Xunit;

namespace Services.Tests.MyHealth
{
    public class EntityDecoratorTests
    {
        private Mock<IHealthRepository> _healthRepository;
        //private HealthService _healthService;
//        private Mock<IConfig> _config;
//        private Mock<ILogger> _logger;

        private Mock<IAggregateStatisticsCalculator> _aggregationCalculator;
        private Mock<ITargetService> _targetService;

        private EntityDecorator _entityDecorator;

        public EntityDecoratorTests()
        {
            _healthRepository = new Mock<IHealthRepository>();
            //_config = new Mock<IConfig>();
            //_logger = new Mock<ILogger>();
            _aggregationCalculator = new Mock<IAggregateStatisticsCalculator>();
            _targetService = new Mock<ITargetService>();

            _entityDecorator = new EntityDecorator(_healthRepository.Object, _aggregationCalculator.Object, _targetService.Object);
        }



        [Fact]
        public void ShouldGetAllActivitySummaries()
        {

            //Given
            var activitySummaries = new List<ActivitySummary>
            {
                new ActivitySummary {CreatedDate = new DateTime(2018,6,6), VeryActiveMinutes = 123}
            };

            var listWithCumSums = new List<ActivitySummary>
            {
                new ActivitySummary{CreatedDate = new DateTime(2000,1,1), CumSumActiveMinutes = 2000},
            };

            var listWithTargets = new List<ActivitySummary>
            {
                new ActivitySummary{CreatedDate = new DateTime(2000,1,1), TargetCumSumActiveMinutes = 2001},
            };

            _healthRepository.Setup(x => x.GetAllActivitySummaries()).Returns(activitySummaries);

            _aggregationCalculator.Setup(x => x.GetCumSums(activitySummaries)).Returns(listWithCumSums);

            _targetService.Setup(x => x.SetTargets(listWithCumSums)).Returns(listWithTargets);

            //when
            var result = _entityDecorator.GetAllActivitySummaries();

            //then
            Assert.Equal(listWithTargets, result);

        }

        [Fact]
        public void ShouldGetAllHeartRateSummaries()
        {

            //Given
            var heartRateSummaries = new List<HeartRateSummary>
            {
                new HeartRateSummary {CreatedDate = new DateTime(2018,6,6), CardioMinutes = 123}
            };

            var listWithCumSums = new List<HeartRateSummary>
            {
                new HeartRateSummary{CreatedDate = new DateTime(2000,1,1), CumSumCardioAndAbove = 2000},
            };

            var listWithTargets = new List<HeartRateSummary>
            {
                new HeartRateSummary{CreatedDate = new DateTime(2000,1,1), TargetCumSumCardioAndAbove = 2001},
            };

            _healthRepository.Setup(x => x.GetAllHeartRateSummaries()).Returns(heartRateSummaries);

            _aggregationCalculator.Setup(x => x.GetCumSums(heartRateSummaries)).Returns(listWithCumSums);

            _targetService.Setup(x => x.SetTargets(listWithCumSums)).Returns(listWithTargets);

            //when
            var result = _entityDecorator.GetAllHeartRateSummaries();

            //then
            Assert.Equal(listWithTargets, result);

        }

        [Fact]
        public void ShouldGetAllRestingHeartRates()
        {

            //Given
            var restingHeartRates = new List<RestingHeartRate>
            {
                new RestingHeartRate {CreatedDate = new DateTime(2018,6,6), Beats = 123}
            };

            var listWithMovingAverages = new List<RestingHeartRate>
            {
                new RestingHeartRate{CreatedDate = new DateTime(2000,1,1), Beats = 2000},
            };

            _healthRepository.Setup(x => x.GetAllRestingHeartRates()).Returns(restingHeartRates);

            _aggregationCalculator.Setup(x => x.GetMovingAverages(restingHeartRates, 10)).Returns(listWithMovingAverages);

            //when
            var result = _entityDecorator.GetAllRestingHeartRates();

            //then
            Assert.Equal(listWithMovingAverages, result);

        }

        [Fact]
        public void ShouldGetAllBloodPressures()
        {

            //Given
            var bloodPressures = new List<BloodPressure>
            {
                new BloodPressure {CreatedDate = new DateTime(2018,6,6), Systolic = 123}
            };

            var listWithMovingAverages = new List<BloodPressure>
            {
                new BloodPressure{CreatedDate = new DateTime(2000,1,1), MovingAverageSystolic = 2000},
            };

            _healthRepository.Setup(x => x.GetAllBloodPressures()).Returns(bloodPressures);

            _aggregationCalculator.Setup(x => x.GetMovingAverages(bloodPressures, 10)).Returns(listWithMovingAverages);

            //when
            var result = _entityDecorator.GetAllBloodPressures();

            //then
            Assert.Equal(listWithMovingAverages, result);

        }

        [Fact]
        public void ShouldGetAllWeights()
        {
            //Given
            var weights = new List<Weight>
            {
                new Weight {CreatedDate = new DateTime(2018,6,6), Kg = 123}
            };

            var listWithMovingAverages = new List<Weight>
            {
                new Weight{CreatedDate = new DateTime(2000,1,1), MovingAverageKg = 2000},
            };

            var listWithTargets = new List<Weight>
            {
                new Weight{CreatedDate = new DateTime(2000,1,1), TargetKg = 2001}
            };

            _healthRepository.Setup(x => x.GetAllWeights()).Returns(weights);

            _aggregationCalculator.Setup(x => x.GetMovingAverages(weights, 10)).Returns(listWithMovingAverages);

            _targetService.Setup(x => x.SetTargets(listWithMovingAverages, 365)).Returns(listWithTargets);

            //when
            var result = _entityDecorator.GetAllWeights();

            //then
            Assert.Equal(listWithTargets, result);

        }

        [Fact]
        public void ShouldGetAllStepCounts()
        {

            //Given
            var stepCounts = new List<StepCount>
            {
                new StepCount {CreatedDate = new DateTime(2018,6,6), Count = 123}
            };

            var listWithCumSums = new List<StepCount>
            {
                new StepCount{CreatedDate = new DateTime(2000,1,1), CumSumCount = 2000},
            };

            var listWithTargets = new List<StepCount>
            {
                new StepCount{CreatedDate = new DateTime(2000,1,1), TargetCumSumCount = 2001},
            };

            _healthRepository.Setup(x => x.GetAllStepCounts()).Returns(stepCounts);

            _aggregationCalculator.Setup(x => x.GetCumSums(stepCounts)).Returns(listWithCumSums);

            _targetService.Setup(x => x.SetTargets(listWithCumSums)).Returns(listWithTargets);

            //when
            var result = _entityDecorator.GetAllStepCounts();

            //then
            Assert.Equal(listWithTargets, result);

        }

        [Fact]
        public void ShouldGetAllAlcoholIntakes()
        {

            //Given
            var alcoholIntakes = new List<AlcoholIntake>
            {
                new AlcoholIntake {CreatedDate = new DateTime(2018,6,6), Units = 123}
            };

            var listWithCumSums = new List<AlcoholIntake>
            {
                new AlcoholIntake(){CreatedDate = new DateTime(2000,1,1), CumSumUnits = 2000},
            };

            var listWithTargets = new List<AlcoholIntake>
            {
                new AlcoholIntake() {CreatedDate = new DateTime(2000, 1, 1), TargetCumSumUnits = 2001},
            };

            _healthRepository.Setup(x => x.GetAllAlcoholIntakes()).Returns(alcoholIntakes);

            _aggregationCalculator.Setup(x => x.GetCumSums(alcoholIntakes)).Returns(listWithCumSums);

            _targetService.Setup(x => x.SetTargets(listWithCumSums)).Returns(listWithTargets);

            //when
            var result = _entityDecorator.GetAllAlcoholIntakes();

            //then
            Assert.Equal(listWithTargets, result);

        }

    }
}