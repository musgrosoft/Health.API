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
        

        public HealthServiceTests()
        {
            _healthRepository = new Mock<IHealthRepository>();
            _config = new Mock<IConfig>();
            _logger = new Mock<ILogger>();
            _aggregationCalculator = new Mock<IAggregationCalculator>();

            _healthService = new HealthService(_config.Object, _logger.Object, _healthRepository.Object, _aggregationCalculator.Object);
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
                new Weight { DateTime = new DateTime(2010,10,10) },
                new Weight { DateTime = new DateTime(2011,10,10) }
            };

            var previousWeights = new List<Weight>();

            var weightsWithAverages = new List<Weight>
            {
                new Weight {DateTime = new DateTime(2016,1,1), Kg = 2016},
                new Weight {DateTime = new DateTime(2017,1,1), Kg = 2017},
                new Weight {DateTime = new DateTime(2018,1,1), Kg = 2018}
            };

            _healthRepository.Setup(x => x.GetLatestWeights(9, new DateTime(2010, 10, 10))).Returns(previousWeights);
            _aggregationCalculator.Setup(x => x.GetMovingAverages(previousWeights, It.IsAny<IList<Weight>>(), 10)).Returns(weightsWithAverages);

            //When
            _healthService.UpsertWeights(newWeights);

            //Then
            _healthRepository.Verify(x => x.Upsert(weightsWithAverages[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(weightsWithAverages[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(weightsWithAverages[2]), Times.Once);
        }

        [Fact]
        public void ShouldUpsertNewBloodPressures()
        {
            //Given
            var newBloodPressures = new List<BloodPressure>
            {
                new BloodPressure { DateTime = new DateTime(2010,10,10) },
                new BloodPressure { DateTime = new DateTime(2011,10,10) }
            };

            var previousBloodPressures = new List<BloodPressure>();

            var bloodPressuresWithAverages = new List<BloodPressure>
            {
                new BloodPressure {DateTime = new DateTime(2016,1,1), Systolic = 2016},
                new BloodPressure {DateTime = new DateTime(2017,1,1), Systolic = 2017},
                new BloodPressure {DateTime = new DateTime(2018,1,1), Systolic = 2018}
            };

            _healthRepository.Setup(x => x.GetLatestBloodPressures(9, new DateTime(2010, 10, 10))).Returns(previousBloodPressures);
            _aggregationCalculator.Setup(x => x.GetMovingAverages(previousBloodPressures, It.IsAny<IList<BloodPressure>>(), 10)).Returns(bloodPressuresWithAverages);

            //When
            _healthService.UpsertBloodPressures(newBloodPressures);

            //Then
            _healthRepository.Verify(x => x.Upsert(bloodPressuresWithAverages[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(bloodPressuresWithAverages[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(bloodPressuresWithAverages[2]), Times.Once);
        }

        [Fact]
        public void ShouldUpsertNewStepCounts()
        {
            //Given
            var newStepCounts = new List<StepCount>
            {
                new StepCount { DateTime = new DateTime(2010,10,10) },
                new StepCount { DateTime = new DateTime(2011,10,10) }
            };

            var previousStepCount = new StepCount();

            var stepCountsWithSums = new List<StepCount>
            {
                new StepCount {DateTime = new DateTime(2016,1,1), Count = 2016},
                new StepCount {DateTime = new DateTime(2017,1,1), Count = 2017},
                new StepCount {DateTime = new DateTime(2018,1,1), Count = 2018}
            };

            _healthRepository.Setup(x => x.GetLatestStepCounts(1, new DateTime(2010, 10, 10))).Returns(new List<StepCount> { previousStepCount });
            _aggregationCalculator.Setup(x => x.GetCumSums(previousStepCount, It.IsAny<IList<StepCount>>())).Returns(stepCountsWithSums);

            //When
            _healthService.UpsertStepCounts(newStepCounts);

            //Then
            _healthRepository.Verify(x => x.Upsert(stepCountsWithSums[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(stepCountsWithSums[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(stepCountsWithSums[2]), Times.Once);
        }

        [Fact]
        public void ShouldUpsertNewActivitySummaries()
        {
            //Given
            var newActivitySummaries = new List<ActivitySummary>
            {
                new ActivitySummary { DateTime = new DateTime(2010,10,10) },
                new ActivitySummary { DateTime = new DateTime(2011,10,10) }
            };

            var previousActivitySummary = new ActivitySummary();
            
            var activitySummariesWithSums = new List<ActivitySummary>
            {
                new ActivitySummary {DateTime = new DateTime(2016,1,1), FairlyActiveMinutes = 2016},
                new ActivitySummary {DateTime = new DateTime(2017,1,1), FairlyActiveMinutes = 2017},
                new ActivitySummary {DateTime = new DateTime(2018,1,1), FairlyActiveMinutes = 2018}
            };

            _healthRepository.Setup(x => x.GetLatestActivitySummaries(1, new DateTime(2010, 10, 10))).Returns(new List<ActivitySummary> { previousActivitySummary });
            _aggregationCalculator.Setup(x => x.GetCumSums(previousActivitySummary, It.IsAny<IList<ActivitySummary>>())).Returns(activitySummariesWithSums);

            //When
            _healthService.UpsertActivitySummaries(newActivitySummaries);

            //Then
            _healthRepository.Verify(x => x.Upsert(activitySummariesWithSums[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(activitySummariesWithSums[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(activitySummariesWithSums[2]), Times.Once);
        }
        
        [Fact]
        public void ShouldUpsertNewRestingHeartRates()
        {
            //Given
            var newHeartSummaries = new List<RestingHeartRate>
            {
                new RestingHeartRate { DateTime = new DateTime(2010,10,10) },
                new RestingHeartRate { DateTime = new DateTime(2011,10,10) }
            };

            var previousRestingHeartRates = new List<RestingHeartRate>();
            
            var restingHeartRatesWithAverages = new List<RestingHeartRate>
            {
                new RestingHeartRate {DateTime = new DateTime(2016,1,1), Beats = 2016},
                new RestingHeartRate {DateTime = new DateTime(2017,1,1), Beats = 2017},
                new RestingHeartRate {DateTime = new DateTime(2018,1,1), Beats = 2018}
            };

            _healthRepository.Setup(x => x.GetLatestRestingHeartRates(9, new DateTime(2010, 10, 10))).Returns(previousRestingHeartRates );
            _aggregationCalculator.Setup(x => x.GetMovingAverages(previousRestingHeartRates, It.IsAny<IList<RestingHeartRate>>(), 10)).Returns(restingHeartRatesWithAverages);

            //When
            _healthService.UpsertRestingHeartRates(newHeartSummaries);

            //Then
            _healthRepository.Verify(x => x.Upsert(restingHeartRatesWithAverages[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(restingHeartRatesWithAverages[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(restingHeartRatesWithAverages[2]), Times.Once);

        }
        
        [Fact]
        public void ShouldUpsertHeartSummaries()
        {
            //Given
            var newHeartSummaries = new List<HeartSummary>
            {
                new HeartSummary { DateTime = new DateTime(2010,10,10) },
                new HeartSummary { DateTime = new DateTime(2011,10,10) }
            };

            var previousHeartSummary = new HeartSummary();

            var heartSummariesWithSums = new List<HeartSummary>
            {
                new HeartSummary {DateTime = new DateTime(2016,1,1), FatBurnMinutes = 2016},
                new HeartSummary {DateTime = new DateTime(2017,1,1), FatBurnMinutes = 2017},
                new HeartSummary {DateTime = new DateTime(2018,1,1), FatBurnMinutes = 2018}
            };

            _healthRepository.Setup(x => x.GetLatestHeartSummaries(1, new DateTime(2010, 10, 10))).Returns(new List<HeartSummary>{previousHeartSummary});
            _aggregationCalculator.Setup(x => x.GetCumSums(previousHeartSummary, It.IsAny<IList<HeartSummary>>())).Returns(heartSummariesWithSums);

            //When
            _healthService.UpsertHeartSummaries(newHeartSummaries);

            //Then
            _healthRepository.Verify(x => x.Upsert(heartSummariesWithSums[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(heartSummariesWithSums[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(heartSummariesWithSums[2]), Times.Once);
        }


    }
}