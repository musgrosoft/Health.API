using System;
using System.Collections.Generic;
using System.Diagnostics;
using Moq;
using Repositories;
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
        public void ShouldInsertNewWeight()
        {
            //Given
            var myNewWeight = new Weight();
            _healthRepository.Setup(x => x.Find(myNewWeight)).Returns((Weight)null);
            _healthRepository.Setup(x => x.GetLatestWeights(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(new List<Weight>());

            //When
            _healthService.UpsertWeights(new List<Weight>{myNewWeight});

            //Then
            _healthRepository.Verify(x=>x.Insert(myNewWeight), Times.Once);
        }

        [Fact]
        public void ShouldUpdateExistingWeight()
        {
            //Given
            var myWeight = new Weight();
            var existingWeight = new Weight();
            _healthRepository.Setup(x => x.Find(myWeight)).Returns(existingWeight);
            _healthRepository.Setup(x => x.GetLatestWeights(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(new List<Weight>());

            //When
            _healthService.UpsertWeights(new List<Weight> { myWeight });

            //Then
            _healthRepository.Verify(x => x.Update(existingWeight, myWeight), Times.Once);
        }

        [Fact]
        public void ShouldInsertNewBloodPressure()
        {
            //Given
            var myBloodPressure = new BloodPressure();
            _healthRepository.Setup(x => x.Find(myBloodPressure)).Returns((BloodPressure)null);

            //When
            _healthService.UpsertBloodPressures(new List<BloodPressure> { myBloodPressure });

            //Then
            _healthRepository.Verify(x => x.Insert(myBloodPressure), Times.Once);
        }

        [Fact]
        public void ShouldUpdateExistingBloodPressure()
        {
            //Given
            var myBloodPressure = new BloodPressure();
            var existingBloodPressure = new BloodPressure();
            _healthRepository.Setup(x => x.Find(myBloodPressure)).Returns(existingBloodPressure);

            //When
            _healthService.UpsertBloodPressures(new List<BloodPressure> { myBloodPressure });

            //Then
            _healthRepository.Verify(x => x.Update(existingBloodPressure, myBloodPressure), Times.Once);
        }

        [Fact]
        public void ShouldInsertNewStepCount()
        {
            //Given
            var myStepCount = new StepCount();
            _healthRepository.Setup(x => x.Find(myStepCount)).Returns((StepCount)null);
            _healthRepository.Setup(x => x.GetLatestStepCounts(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(new List<StepCount>());

            //When
            _healthService.UpsertStepCounts(new List<StepCount> { myStepCount });

            //Then
            _healthRepository.Verify(x => x.Insert(myStepCount), Times.Once);
        }

        [Fact]
        public void ShouldUpdateExistingStepCount()
        {
            //Given
            var myStepCount = new StepCount();
            var existingStepCount = new StepCount();
            _healthRepository.Setup(x => x.Find(myStepCount)).Returns(existingStepCount);
            _healthRepository.Setup(x => x.GetLatestStepCounts(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(new List<StepCount>());

            //When
            _healthService.UpsertStepCounts(new List<StepCount> { myStepCount });

            //Then
            _healthRepository.Verify(x => x.Update(existingStepCount, myStepCount), Times.Once);
        }


        [Fact]
        public void ShouldInsertNewActivitySummary()
        {
            //Given
            var myActivitySummary = new ActivitySummary();
            _healthRepository.Setup(x => x.Find(myActivitySummary)).Returns((ActivitySummary)null);
            _healthRepository.Setup(x => x.GetLatestActivitySummaries(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(new List<ActivitySummary>());

            //When
            _healthService.UpsertActivitySummaries(new List<ActivitySummary> { myActivitySummary });

            //Then
            _healthRepository.Verify(x => x.Insert(myActivitySummary), Times.Once);
        }

        [Fact]
        public void ShouldUpdateExistingActivitySummary()
        {
            //Given
            var myActivitySummary = new ActivitySummary();
            var existingActivitySummary = new ActivitySummary();
            _healthRepository.Setup(x => x.Find(myActivitySummary)).Returns(existingActivitySummary);
            _healthRepository.Setup(x => x.GetLatestActivitySummaries(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(new List<ActivitySummary>());

            //When
            _healthService.UpsertActivitySummaries(new List<ActivitySummary> { myActivitySummary });

            //Then
            _healthRepository.Verify(x => x.Update(existingActivitySummary, myActivitySummary), Times.Once);
        }

        [Fact]
        public void ShouldInsertNewRestingHeartRate()
        {
            //Given
            var myRestingHeartRate = new RestingHeartRate();
            _healthRepository.Setup(x => x.Find(myRestingHeartRate)).Returns((RestingHeartRate)null);

            //When
            _healthService.UpsertRestingHeartRates(new List<RestingHeartRate> { myRestingHeartRate });

            //Then
            _healthRepository.Verify(x => x.Insert(myRestingHeartRate), Times.Once);
        }

        [Fact]
        public void ShouldUpdateExistingRestingHeartRate()
        {
            //Given
            var myRestingHeartRate = new RestingHeartRate();
            var existingRestingHeartRate = new RestingHeartRate();
            _healthRepository.Setup(x => x.Find(myRestingHeartRate)).Returns(existingRestingHeartRate);

            //When
            _healthService.UpsertRestingHeartRates(new List<RestingHeartRate> { myRestingHeartRate });

            //Then
            _healthRepository.Verify(x => x.Update(existingRestingHeartRate, myRestingHeartRate), Times.Once);
        }

        [Fact]
        public void ShouldInsertNewHeartSummary()
        {
            //Given
            var myHeartSummary = new HeartSummary();
            _healthRepository.Setup(x => x.Find(myHeartSummary)).Returns((HeartSummary)null);
            _healthRepository.Setup(x => x.GetLatestHeartSummaries(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(new List<HeartSummary>());

            //When
            _healthService.UpsertHeartSummaries(new List<HeartSummary> { myHeartSummary });

            //Then
            _healthRepository.Verify(x => x.Insert(myHeartSummary), Times.Once);
        }

        [Fact]
        public void ShouldUpdateExistingHeartSummary()
        {
            //Given
            var myHeartSummary = new HeartSummary();
            var existingHeartSummary = new HeartSummary();
            _healthRepository.Setup(x => x.Find(myHeartSummary)).Returns(existingHeartSummary);
            _healthRepository.Setup(x => x.GetLatestHeartSummaries(It.IsAny<int>(), It.IsAny<DateTime>()))
                .Returns(new List<HeartSummary>());

            //When
            _healthService.UpsertHeartSummaries(new List<HeartSummary> { myHeartSummary });

            //Then
            _healthRepository.Verify(x => x.Update(existingHeartSummary, myHeartSummary), Times.Once);
        }

        



    }
}