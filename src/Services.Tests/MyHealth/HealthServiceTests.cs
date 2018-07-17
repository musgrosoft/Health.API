using System;
using System.Collections.Generic;
using Moq;
using Repositories.Health;
using Repositories.Models;
using Services.MyHealth;
using Utils;
using Xunit;

namespace Services.Tests.MyHealth
{
    public class HealthServiceTests
    {
        private Mock<IHealthRepository> _healthRepository;
        private HealthService _healthService;
        private Mock<IConfig> _config;
        private Mock<ILogger> _logger;

        private Mock<IAggregationCalculator> _aggregationCalculator;
        private Mock<ITargetService> _targetService;


        public HealthServiceTests()
        {
            _healthRepository = new Mock<IHealthRepository>();
            _config = new Mock<IConfig>();
            _logger = new Mock<ILogger>();
            _aggregationCalculator = new Mock<IAggregationCalculator>();
            _targetService = new Mock<ITargetService>();

            _healthService = new HealthService(_config.Object, _logger.Object, _healthRepository.Object, _aggregationCalculator.Object, _targetService.Object);
        }

        [Fact]
        public void ShouldGetLatestWeightDate()
        {
            //Given 
            var date = new DateTime(2018, 1, 2);
            _healthRepository.Setup(x => x.GetLatestWeightDate()).Returns(date);

            //when
            var latestDate = _healthService.GetLatestWeightDate(DateTime.MinValue);

            //then
            Assert.Equal(date, latestDate);
        }


        [Fact]
        public void ShouldGetLatestStepCountDate()
        {
            //Given 
            var date = new DateTime(2018, 1, 4);
            _healthRepository.Setup(x => x.GetLatestStepCountDate()).Returns(date);

            //when
            var latestDate = _healthService.GetLatestStepCountDate(DateTime.MinValue);

            //then
            Assert.Equal(date, latestDate);

        }

        [Fact]
        public void ShouldGetLatestActivitySummaryDate()
        {
            //Given 
            var date = new DateTime(2018, 1, 5);
            _healthRepository.Setup(x => x.GetLatestActivitySummaryDate()).Returns(date);

            //when
            var latestDate = _healthService.GetLatestActivitySummaryDate(DateTime.MinValue);

            //then
            Assert.Equal(date, latestDate);

        }


        [Fact]
        public void ShouldGetLatestRestingHeartRateDate()
        {
            //Given 
            var date = new DateTime(2018, 1, 6);
            _healthRepository.Setup(x => x.GetLatestRestingHeartRateDate()).Returns(date);

            //when
            var latestDate = _healthService.GetLatestRestingHeartRateDate(DateTime.MinValue);

            //then
            Assert.Equal(date, latestDate);

        }

        [Fact]
        public void ShouldGetLatestHeartSummaryDate()
        {
            //Given 
            var date = new DateTime(2018, 1, 7);
            _healthRepository.Setup(x => x.GetLatestHeartSummaryDate()).Returns(date);

            //when
            var latestDate = _healthService.GetLatestHeartSummaryDate(DateTime.MinValue);

            //then
            Assert.Equal(date, latestDate);

        }

        [Fact]
        public void ShouldUpsertNewWeights()
        {
            //Given
            var newWeights = new List<Weight>
            {
                new Weight { CreatedDate = new DateTime(2010,10,10) },
                new Weight { CreatedDate = new DateTime(2010,10,11) },
                new Weight { CreatedDate = new DateTime(2010,10,12) }

            };

            //When
            _healthService.UpsertWeights(newWeights);

            //Then
            _healthRepository.Verify(x => x.Upsert(newWeights[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newWeights[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newWeights[2]), Times.Once);
        }

        [Fact]
        public void ShouldUpsertNewBloodPressures()
        {
            //Given
            var newBloodPressures = new List<BloodPressure>
            {
                new BloodPressure { CreatedDate = new DateTime(2010,10,10) },
                new BloodPressure { CreatedDate = new DateTime(2010,10,11) },
                new BloodPressure { CreatedDate = new DateTime(2010,10,12) }
            };
            
            //When
            _healthService.UpsertBloodPressures(newBloodPressures);

            //Then
            _healthRepository.Verify(x => x.Upsert(newBloodPressures[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newBloodPressures[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newBloodPressures[2]), Times.Once);
        }

        [Fact]
        public void ShouldUpsertNewStepCounts()
        {
            //Given
            var newStepCounts = new List<StepCount>
            {
                new StepCount {CreatedDate = new DateTime(2016,1,1), Count = 2016},
                new StepCount {CreatedDate = new DateTime(2017,1,1), Count = 2017},
                new StepCount {CreatedDate = new DateTime(2018,1,1), Count = 2018}
            };

            //When
            _healthService.UpsertStepCounts(newStepCounts);

            //Then
            _healthRepository.Verify(x => x.Upsert(newStepCounts[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newStepCounts[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newStepCounts[2]), Times.Once);
        }

        [Fact]
        public void ShouldUpsertNewActivitySummaries()
        {
            //Given
            var newActivitySummaries = new List<ActivitySummary>
            {
                new ActivitySummary {CreatedDate = new DateTime(2016,1,1), FairlyActiveMinutes = 2016},
                new ActivitySummary {CreatedDate = new DateTime(2017,1,1), FairlyActiveMinutes = 2017},
                new ActivitySummary {CreatedDate = new DateTime(2018,1,1), FairlyActiveMinutes = 2018}
            };
            
            //When
            _healthService.UpsertActivitySummaries(newActivitySummaries);

            //Then
            _healthRepository.Verify(x => x.Upsert(newActivitySummaries[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newActivitySummaries[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newActivitySummaries[2]), Times.Once);
        }
        
        [Fact]
        public void ShouldUpsertNewRestingHeartRates()
        {
            //Given
            var newHeartSummaries = new List<RestingHeartRate>
            {
                new RestingHeartRate { CreatedDate = new DateTime(2010,10,10) },
                new RestingHeartRate { CreatedDate = new DateTime(2010,10,11) },
                new RestingHeartRate { CreatedDate = new DateTime(2010,10,12) }
            };
            
            //When
            _healthService.UpsertRestingHeartRates(newHeartSummaries);

            //Then
            _healthRepository.Verify(x => x.Upsert(newHeartSummaries[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newHeartSummaries[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newHeartSummaries[2]), Times.Once);

        }
        
        [Fact]
        public void ShouldUpsertHeartSummaries()
        {
            //Given
            var newHeartSummaries = new List<HeartRateSummary>
            {
                new HeartRateSummary {CreatedDate = new DateTime(2016,1,1), FatBurnMinutes = 2016},
                new HeartRateSummary {CreatedDate = new DateTime(2017,1,1), FatBurnMinutes = 2017},
                new HeartRateSummary {CreatedDate = new DateTime(2018,1,1), FatBurnMinutes = 2018}
            };

            //When
            _healthService.UpsertHeartSummaries(newHeartSummaries);

            //Then
            _healthRepository.Verify(x => x.Upsert(newHeartSummaries[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newHeartSummaries[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newHeartSummaries[2]), Times.Once);
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
            var result = _healthService.GetAllRestingHeartRates();

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
            var result = _healthService.GetAllBloodPressures();

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
            var result = _healthService.GetAllWeights();

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
            var result = _healthService.GetAllStepCounts();

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
            var result = _healthService.GetAllAlcoholIntakes();

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
            var result = _healthService.GetAllHeartRateSummaries();

            //then
            Assert.Equal(listWithTargets, result);

        }

    }
}