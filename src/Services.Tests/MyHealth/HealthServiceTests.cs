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

        [Fact]
        public void ShouldDoMovingAverages()
        {
            var kgs = new List<decimal> {10, 20, 30, 40, 50, 60, 70, 80, 90};
            var orderedWeights = new List<Weight>
            {
                new Weight {Kg = 100},
                new Weight {Kg = 110},
                new Weight {Kg = 120},
                new Weight {Kg = 130},
                new Weight {Kg = 140},
                new Weight {Kg = 150},
                new Weight {Kg = 160},
                new Weight {Kg = 170},
                new Weight {Kg = 180},
                new Weight {Kg = 190},
                new Weight {Kg = 200},
                new Weight {Kg = 210},
                new Weight {Kg = 220},
                new Weight {Kg = 230},
                new Weight {Kg = 240},
            };

            _healthService.SetMovingAveragesForWeights(kgs,orderedWeights, 10);

            Assert.Equal(15, orderedWeights.Count);

            Assert.Equal(100, orderedWeights[0].Kg);
            Assert.Equal(110, orderedWeights[1].Kg);
            Assert.Equal(120, orderedWeights[2].Kg);
            Assert.Equal(130, orderedWeights[3].Kg);
            Assert.Equal(140, orderedWeights[4].Kg);
            Assert.Equal(150, orderedWeights[5].Kg);
            Assert.Equal(160, orderedWeights[6].Kg);
            Assert.Equal(170, orderedWeights[7].Kg);
            Assert.Equal(180, orderedWeights[8].Kg);
            Assert.Equal(190, orderedWeights[9].Kg);
            Assert.Equal(200, orderedWeights[10].Kg);
            Assert.Equal(210, orderedWeights[11].Kg);
            Assert.Equal(220, orderedWeights[12].Kg);
            Assert.Equal(230, orderedWeights[13].Kg);
            Assert.Equal(240, orderedWeights[14].Kg);

            Assert.Equal(55, orderedWeights[0].MovingAverageKg);
            Assert.Equal(65, orderedWeights[1].MovingAverageKg);
            Assert.Equal(75, orderedWeights[2].MovingAverageKg);
            Assert.Equal(85, orderedWeights[3].MovingAverageKg);
            Assert.Equal(95, orderedWeights[4].MovingAverageKg);
            Assert.Equal(105, orderedWeights[5].MovingAverageKg);
            Assert.Equal(115, orderedWeights[6].MovingAverageKg);
            Assert.Equal(125, orderedWeights[7].MovingAverageKg);
            Assert.Equal(135, orderedWeights[8].MovingAverageKg);
            Assert.Equal(145, orderedWeights[9].MovingAverageKg);
            Assert.Equal(155, orderedWeights[10].MovingAverageKg);
            Assert.Equal(165, orderedWeights[11].MovingAverageKg);
            Assert.Equal(175, orderedWeights[12].MovingAverageKg);
            Assert.Equal(185, orderedWeights[13].MovingAverageKg);
            Assert.Equal(195, orderedWeights[14].MovingAverageKg);
            
        }


        [Fact]
        public void ShouldDoMovingAverages2()
        {
            var kgs = new List<decimal> { 40, 50, 60, 70, 80, 90 };
            var orderedWeights = new List<Weight>
            {
                new Weight {Kg = 100},
                new Weight {Kg = 110},
                new Weight {Kg = 120},
                new Weight {Kg = 130},
                new Weight {Kg = 140},
                new Weight {Kg = 150},
                new Weight {Kg = 160},
                new Weight {Kg = 170},
                new Weight {Kg = 180},
                new Weight {Kg = 190},
                new Weight {Kg = 200},
                new Weight {Kg = 210},
                new Weight {Kg = 220},
                new Weight {Kg = 230},
                new Weight {Kg = 240},
            };

            _healthService.SetMovingAveragesForWeights(kgs, orderedWeights, 10);

            Assert.Equal(15, orderedWeights.Count);

            Assert.Equal(100, orderedWeights[0].Kg);
            Assert.Equal(110, orderedWeights[1].Kg);
            Assert.Equal(120, orderedWeights[2].Kg);
            Assert.Equal(130, orderedWeights[3].Kg);
            Assert.Equal(140, orderedWeights[4].Kg);
            Assert.Equal(150, orderedWeights[5].Kg);
            Assert.Equal(160, orderedWeights[6].Kg);
            Assert.Equal(170, orderedWeights[7].Kg);
            Assert.Equal(180, orderedWeights[8].Kg);
            Assert.Equal(190, orderedWeights[9].Kg);
            Assert.Equal(200, orderedWeights[10].Kg);
            Assert.Equal(210, orderedWeights[11].Kg);
            Assert.Equal(220, orderedWeights[12].Kg);
            Assert.Equal(230, orderedWeights[13].Kg);
            Assert.Equal(240, orderedWeights[14].Kg);

            Assert.Null(orderedWeights[0].MovingAverageKg);
            Assert.Null(orderedWeights[1].MovingAverageKg);
            Assert.Null(orderedWeights[2].MovingAverageKg);
            Assert.Equal(85, orderedWeights[3].MovingAverageKg);
            Assert.Equal(95, orderedWeights[4].MovingAverageKg);
            Assert.Equal(105, orderedWeights[5].MovingAverageKg);
            Assert.Equal(115, orderedWeights[6].MovingAverageKg);
            Assert.Equal(125, orderedWeights[7].MovingAverageKg);
            Assert.Equal(135, orderedWeights[8].MovingAverageKg);
            Assert.Equal(145, orderedWeights[9].MovingAverageKg);
            Assert.Equal(155, orderedWeights[10].MovingAverageKg);
            Assert.Equal(165, orderedWeights[11].MovingAverageKg);
            Assert.Equal(175, orderedWeights[12].MovingAverageKg);
            Assert.Equal(185, orderedWeights[13].MovingAverageKg);
            Assert.Equal(195, orderedWeights[14].MovingAverageKg);

        }



    }
}